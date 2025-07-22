using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Services.Email
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otp);
    }
}
