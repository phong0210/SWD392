using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserCreateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
    }
} 