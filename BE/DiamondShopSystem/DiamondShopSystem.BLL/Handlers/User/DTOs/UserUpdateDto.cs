using System.ComponentModel.DataAnnotations;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Address { get; set; } = string.Empty;
    }
} 