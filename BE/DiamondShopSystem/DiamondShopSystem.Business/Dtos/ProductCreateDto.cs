using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.Business.Dtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU is required.")]
        [StringLength(50, ErrorMessage = "SKU cannot be longer than 50 characters.")]
        public string SKU { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Description cannot be longer than 255 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
       [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        public int? Carat { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Clarity { get; set; }

        [StringLength(50)]
        public string? Cut { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        public string? GIACertNumber { get; set; }

        public bool IsHidden { get; set; } = false;

        [Required(ErrorMessage = "Category is required.")]
        public Guid CategoryId { get; set; }
    }
}