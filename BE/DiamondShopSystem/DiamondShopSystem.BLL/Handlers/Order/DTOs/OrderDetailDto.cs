using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.Order.DTOs
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; } = default!;
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
    }
} 