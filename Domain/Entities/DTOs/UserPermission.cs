using Domain.Entities.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DTOs
{

    [ConnectionName("Security")]
    public class UserPermission
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
