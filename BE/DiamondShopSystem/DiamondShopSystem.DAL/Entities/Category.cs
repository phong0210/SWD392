using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiamondShopSystem.DAL.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;

        [InverseProperty("Category")]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
} 