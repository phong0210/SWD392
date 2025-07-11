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

        [Required]
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;

        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;

        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
} 