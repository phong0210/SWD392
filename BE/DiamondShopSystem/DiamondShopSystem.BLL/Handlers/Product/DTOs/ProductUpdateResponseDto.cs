using System;

namespace DiamondShopSystem.BLL.Handlers.Product.DTOs
{
    public class ProductUpdateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ProductInfoDto? Product { get; set; }
    }
}