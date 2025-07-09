using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Handlers.Product.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }

        public int? Carat { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Clarity { get; set; }

        [StringLength(50)]
        public string? Cut { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string? GIACertNumber { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
