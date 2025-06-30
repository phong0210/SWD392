using DiamondShopSystem.BLL.Application.DTOs;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<LoginResponse> GoogleAuthenticateAsync(string idToken);
    }
}