using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserGetResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public UserAccountInfoDto? User { get; set; }
    }

    public class UserAccountInfoDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; } = string.Empty;
    }
} 