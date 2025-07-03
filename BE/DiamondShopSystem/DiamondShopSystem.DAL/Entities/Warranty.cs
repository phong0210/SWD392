using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Warranty
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public DateTime WarrantyStart { get; set; }
        public DateTime WarrantyEnd { get; set; }

        [StringLength(255)]
        public string Details { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
} 