
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Application.DTOs
{
    public class UpdateProductRequest
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Sku { get; set; }

        [Range(0.01, double.MaxValue)]
        public float? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        [StringLength(50)]
        public string? GiaCertificationNumber { get; set; }

        public bool? IsHidden { get; set; }

        [Range(0.01, double.MaxValue)]
        public float? Carat { get; set; }

        [StringLength(20)]
        public string? Color { get; set; }

        [StringLength(20)]
        public string? Clarity { get; set; }

        [StringLength(20)]
        public string? Cut { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
