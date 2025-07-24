namespace DiamondShopSystem.BLL.Handlers.Auth.DTOs
{
    public class ConfirmPasswordResetDto
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
        public required string NewPassword { get; set; }
    }
}
