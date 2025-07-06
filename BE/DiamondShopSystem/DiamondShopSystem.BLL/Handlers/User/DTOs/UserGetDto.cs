using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RoleName { get; set; }
    }
} 