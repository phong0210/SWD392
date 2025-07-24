namespace DiamondShopSystem.BLL.Handlers.Auth.DTOs
{
    public class VerifyOtpRequestDto
    {
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}
