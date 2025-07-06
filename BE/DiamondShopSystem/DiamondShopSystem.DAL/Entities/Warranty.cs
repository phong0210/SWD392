using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Warranty
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        [Required]
        public DateTime WarrantyStart { get; set; }
        [Required]
        public DateTime WarrantyEnd { get; set; }
        [Required]
        [StringLength(255)]
        public string Details { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }
    }
} 