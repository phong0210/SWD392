using System;

namespace DiamondShopSystem.BLL.Handlers.Product.DTOs
{
    public class ProductGetResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ProductInfoDto? Product { get; set; }
    }

    public class ProductInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int? Carat { get; set; }
        public string? Color { get; set; }
        public string? Clarity { get; set; }
        public string? Cut { get; set; }
        public int StockQuantity { get; set; }
        public string? GIACertNumber { get; set; }
        public bool IsHidden { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? OrderDetailId { get; set; }
        public Guid? WarrantyId { get; set; }
    }
}