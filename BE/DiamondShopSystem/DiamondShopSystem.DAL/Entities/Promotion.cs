using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.DAL.Entities
{
    public class Promotion
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string DiscountType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DiscountValue { get; set; } = string.Empty;

        public Guid? AppliesToProductId { get; set; }
        public Product? Product { get; set; }
    }
} 