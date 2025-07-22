using System;

namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class ConfirmRegistrationDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}