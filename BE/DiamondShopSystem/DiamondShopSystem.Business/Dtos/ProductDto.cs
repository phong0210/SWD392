using System;

namespace DiamondShopSystem.Business.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int? Carat { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Clarity { get; set; } = string.Empty;
        public string Cut { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public string GIACertNumber { get; set; } = string.Empty;
        public bool IsHidden { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}