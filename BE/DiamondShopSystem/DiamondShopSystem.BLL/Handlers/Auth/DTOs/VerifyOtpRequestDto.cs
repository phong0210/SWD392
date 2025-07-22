namespace DiamondShopSystem.BLL.Handlers.Auth.DTOs
{
    public class VerifyOtpRequestDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
