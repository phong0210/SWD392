namespace DiamondShopSystem.BLL.Handlers.User.DTOs
{
    public class UserRegisterResponseDto
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Email { get; set; }
    }
}