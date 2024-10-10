using Domain.Entities.CustomAttributes;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Domain.Entities
{
    [ConnectionName("Security")]
    public class User
    {
        [Key]
        public int Id_User { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Create_User { get; set; }
        public DateTime Create_Date { get; set; }
        public string? Modify_User { get; set; }
        public DateTime? Modify_Date { get; set; }
        public bool Active { get; set; }

    }
    public class LoginModel
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Nombre del Modulo")]
        public string Module { get; set; }
    }
}
