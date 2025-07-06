using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public Guid? GoogleId { get; set; }
    }
} 