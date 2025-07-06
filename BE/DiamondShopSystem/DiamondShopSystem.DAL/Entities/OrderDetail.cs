using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Entities
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
} 