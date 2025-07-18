using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Handlers.Category.DTOs
{
    
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
    }
}