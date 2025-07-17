using System;
using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Handlers.Warranty.DTOs
{
    public class WarrantyUpdateDto
    {
        [Required]
        public Guid ProductId { get; set; }
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