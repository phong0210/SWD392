using System;

namespace DiamondShopSystem.BLL.Handlers.Warranty.DTOs
{
    public class WarrantyInfoDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public DateTime WarrantyStart { get; set; }
        public DateTime WarrantyEnd { get; set; }
        public string Details { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}