﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Warranty.DTOs
{
    public class WarrantyCreateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? WarrantyId { get; set; }
       
    }
}
