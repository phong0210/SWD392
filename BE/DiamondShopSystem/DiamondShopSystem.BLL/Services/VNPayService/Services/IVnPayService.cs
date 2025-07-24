using DiamondShopSystem.BLL.Services.Implements.VNPayService.Models;
using Microsoft.AspNetCore.Http;

namespace DiamondShopSystem.BLL.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
