﻿using Domain.Entities.CustomAttributes;
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
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }
}