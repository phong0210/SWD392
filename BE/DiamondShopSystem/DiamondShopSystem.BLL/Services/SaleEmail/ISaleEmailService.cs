using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.BLL.Services.SaleEmail
{
    public interface ISaleEmailService
    {
        Task SendOrderUpdateEmailAsync(string recipientEmail, OrderResponseDto orderDetails);
    }
}
