using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserUpdateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public UserAccountInfoDto? User { get; set; }
    }
} 