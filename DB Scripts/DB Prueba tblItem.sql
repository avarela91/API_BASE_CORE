USE [pruebas]
GO
/****** Object:  StoredProcedure [dbo].[GenerateCRUDProcedures_Print]    Script Date: 08/06/24 14:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Procedimiento almacenado que genera scripts para los procedimientos del CRUD
CREATE PROCEDURE [dbo].[GenerateCRUDProcedures_Print]
    @TableName NVARCHAR(255)
AS
BEGIN

--///////////////////////////////////////////////////////////////////////////////
--/////////////////PROCEDIMIENTO SELECT/////////////////////////////////////////
--//////////////////////////////////////////////////////////////////////////////
DECLARE @Columns NVARCHAR(MAX);
DECLARE @ParamListSelect NVARCHAR(MAX);
DECLARE @WhereConditions NVARCHAR(MAX);

-- Obtener información de columnas
SELECT @Columns = COALESCE(@Columns + ',' + CHAR(10), '') + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @TableName
ORDER BY ORDINAL_POSITION;


SET @ParamListSelect = STUFF((
    SELECT CHAR(10) + ',@' + COLUMN_NAME + ' ' + 
           DATA_TYPE +
           CASE 
               WHEN DATA_TYPE IN ('varchar', 'char', 'nvarchar', 'nchar') THEN '(' + CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(10)) + ')'
               ELSE ''
           END + ' = NULL'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = @TableName
    ORDER BY ORDINAL_POSITION
    FOR XML PATH('')
), 1, 2, '');


SET @WhereConditions = STUFF((
    SELECT CHAR(10) + '   ' + IIF(ROW_NUMBER() OVER (ORDER BY ORDINAL_POSITION) = 1, '', 'AND ') +
        '(@' + COLUMN_NAME + ' IS NULL OR @' + COLUMN_NAME + ' = ' + COLUMN_NAME + ')'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = @TableName
    ORDER BY ORDINAL_POSITION
    FOR XML PATH('')
), 1, 4, '');


PRINT '
-- Procedimiento para select
CREATE PROCEDURE [dbo].[' + @TableName + '_Select]
' + @ParamListSelect + '

AS
BEGIN
    SELECT ' + CHAR(10) + @Columns + '
    FROM ' + @TableName + CHAR(10) + '    WHERE ' + @WhereConditions + '
END
';
END


--///////////////////////////////////////////////////////////////////////////////
--/////////////////PROCEDIMIENTO INSERT/////////////////////////////////////////
--//////////////////////////////////////////////////////////////////////////////


DECLARE @ColumnsValues NVARCHAR(MAX);
DECLARE @ParamListInsert NVARCHAR(MAX);



SELECT @ColumnsValues = COALESCE(@ColumnsValues + ',' + CHAR(10), '') + '@' + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @TableName
ORDER BY ORDINAL_POSITION;


SET @ParamListInsert = STUFF((
    SELECT CHAR(10) + ', @' + COLUMN_NAME + ' ' + 
           DATA_TYPE +
           CASE 
               WHEN DATA_TYPE IN ('varchar', 'char', 'nvarchar', 'nchar') THEN '(' + CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(10)) + ')'
               ELSE ''
           END+ ' = NULL'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME =@TableName
    ORDER BY ORDINAL_POSITION
    FOR XML PATH('')
), 1, 2, '');


PRINT '
-- Procedimiento para insert
CREATE PROCEDURE [dbo].[' + @TableName + '_Insert]
' + @ParamListInsert + '
AS
BEGIN
    INSERT INTO '+@TableName+' (
        ' + @Columns + '
    )
    VALUES (
      ' + @ColumnsValues + '
    );
END
Select @@IDENTITY
';


--///////////////////////////////////////////////////////////////////////////////
--/////////////////PROCEDIMIENTO UPDATE/////////////////////////////////////////
--//////////////////////////////////////////////////////////////////////////////


DECLARE @ParamListUpdate NVARCHAR(MAX);
DECLARE @SetStatements NVARCHAR(MAX);


SELECT @Columns = COALESCE(@Columns + ',' + CHAR(10), '') + COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @TableName
ORDER BY ORDINAL_POSITION;


SET @ParamListUpdate = STUFF((
    SELECT CHAR(10) + ', @' + COLUMN_NAME + ' ' + 
           DATA_TYPE +
           CASE 
               WHEN DATA_TYPE IN ('varchar', 'char', 'nvarchar', 'nchar') THEN '(' + CAST(CHARACTER_MAXIMUM_LENGTH AS NVARCHAR(10)) + ')'
               ELSE ''
           END+ ' = NULL'
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = @TableName
    ORDER BY ORDINAL_POSITION
    FOR XML PATH('')
), 1, 2, '');


SET @SetStatements = STUFF((
    SELECT CHAR(10) + '    ' + COLUMN_NAME + ' = @' + COLUMN_NAME + ','
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = @TableName
    ORDER BY ORDINAL_POSITION
    FOR XML PATH('')
), 1, 4, '');


PRINT '
-- Procedimiento para update
CREATE PROCEDURE [dbo].[' + @TableName + '_Update]
' + @ParamListUpdate + '
AS

UPDATE '+@TableName+'
SET 
' + @SetStatements + '
WHERE 
    (@Id_'+@TableName+' = Id_'+@TableName+');
';

GO
/****** Object:  Table [dbo].[Item]    Script Date: 08/06/24 14:50:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Item](
	[ItemCode] [varchar](50) NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
