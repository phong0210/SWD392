using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;

        [StringLength(50)]
        public string Method { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
    }
} 