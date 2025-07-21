using System;

namespace DiamondShopSystem.BLL.Handlers.Warranty.DTOs
{
    public class WarrantyUpdateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public WarrantyInfoDto? Warranty { get; set; }
    }
}