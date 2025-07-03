using System;

namespace DiamondShopSystem.BLL.Handlers.User
{
    public class UserCreateResponseDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
} 