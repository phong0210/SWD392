namespace DiamondShopSystem.BLL.Handlers.Auth.DTOs
{
    public class ConfirmPasswordResetDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
    }
}
