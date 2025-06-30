using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Services.Authentication
{
    public interface IGoogleOAuthService
    {
        Task<GoogleUserInfo?> ValidateIdTokenAsync(string idToken);
    }

    public class GoogleUserInfo
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }
    }
} 