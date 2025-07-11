using System;
using System.Collections.Generic;

namespace DiamondShopSystem.BLL.Handlers.Order.DTOs
{
    public class OrderDetailCreateDto
    {
        public Guid OrderId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
    }
} 