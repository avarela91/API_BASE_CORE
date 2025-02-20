using Domain.Entities.CustomAttributes;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [ConnectionName("DefaultConnection")]
    public class Item
    {
        [Key]
        public int Id_Item { get; set; }
        public int Id_Department { get; set; }
        public int Id_Category { get; set; }
        public int Id_Supplier { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public string? CodeBars { get; set; }
        public decimal ItemCost { get; set; }
        public string? Create_User { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Create_Date { get; set; }
        public string? Modify_User { get; set; }
        public DateTime? Modify_Date { get; set; }
        public bool Active { get; set; }
    }
}
