
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Sku { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public float Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        [StringLength(50)]
        public string GiaCertificationNumber { get; set; }

        public bool IsHidden { get; set; } = false;

        [Required]
        [Range(0.01, double.MaxValue)]
        public float Carat { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; }

        [Required]
        [StringLength(20)]
        public string Clarity { get; set; }

        [Required]
        [StringLength(20)]
        public string Cut { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
