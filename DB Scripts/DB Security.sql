USE [Security]
GO
/****** Object:  StoredProcedure [dbo].[GenerateCRUDProcedures_Print]    Script Date: 08/06/24 14:49:24 ******/
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
           END
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
           END
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
/****** Object:  Table [dbo].[Module]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Module](
	[Id_Module] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](255) NULL,
	[Create_User] [varchar](50) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](50) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Module] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permission](
	[Id_Permission] [int] IDENTITY(1,1) NOT NULL,
	[Id_Module] [int] NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](100) NULL,
	[Create_User] [varchar](255) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](255) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Permission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[Id_Role] [int] IDENTITY(1,1) NOT NULL,
	[Id_Module] [int] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Create_User] [varchar](255) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](255) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role_Permission]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role_Permission](
	[Id_Role_Permission] [int] IDENTITY(1,1) NOT NULL,
	[Id_Role] [int] NULL,
	[Id_Permission] [int] NULL,
	[Create_User] [varchar](255) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](255) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Id_User] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Create_User] [varchar](50) NOT NULL,
	[Create_Date] [datetime] NOT NULL,
	[Modify_User] [varchar](50) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id_User] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User_Permission]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User_Permission](
	[Id_User_Permission] [int] IDENTITY(1,1) NOT NULL,
	[Id_User] [int] NOT NULL,
	[Id_Permission] [int] NOT NULL,
	[Id_Module] [int] NOT NULL,
	[Create_User] [varchar](50) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](50) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_User_Permission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 08/06/24 14:49:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User_Role](
	[Id_User_Role] [int] IDENTITY(1,1) NOT NULL,
	[Id_User] [int] NOT NULL,
	[Id_Role] [int] NOT NULL,
	[Id_Module] [int] NOT NULL,
	[Create_User] [varchar](255) NULL,
	[Create_Date] [datetime] NULL,
	[Modify_User] [varchar](255) NULL,
	[Modify_Date] [datetime] NULL,
	[Active] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_User_Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
