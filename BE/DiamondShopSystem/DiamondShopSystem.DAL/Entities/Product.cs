using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        [Required]

        public double Price { get; set; }
        public int? Carat { get; set; }

        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        [StringLength(50)]
        public string Clarity { get; set; } = string.Empty;

        [StringLength(50)]
        public string Cut { get; set; } = string.Empty;

        [Required]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string GIACertNumber { get; set; } = string.Empty;

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public Guid? OrderDetailId { get; set; }
        public OrderDetail? OrderDetail { get; set; }

        public Warranty? Warranty { get; set; }
    }
}