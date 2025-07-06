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

        public double Price { get; set; }
        public int? Carat { get; set; }

        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        [StringLength(50)]
        public string Clarity { get; set; } = string.Empty;

        [StringLength(50)]
        public string Cut { get; set; } = string.Empty;

        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string GIACertNumber { get; set; } = string.Empty;

        public bool IsHidden { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        [InverseProperty("Product")]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public Warranty? Warranty { get; set; }
    }
} 