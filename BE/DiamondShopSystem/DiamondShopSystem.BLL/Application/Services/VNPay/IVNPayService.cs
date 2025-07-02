using DiamondShopSystem.BLL.Application.DTOs;
using System.Threading.Tasks;

namespace DiamondShopSystem.BLL.Application.Services.VNPay
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(CreateVNPayPaymentRequest request);
        bool ProcessPaymentCallback(VNPayCallbackRequest request);
    }
}