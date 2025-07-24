using DiamondShopSystem.BLL.Services.Implements.VNPayService.Models;
using DiamondShopSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            return Ok(new { url });
        }

        [HttpGet("payment-callback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                var successUrl = Request.Query["success_url"].ToString();
                return Redirect(successUrl);
            }
            else
            {
                var failUrl = Request.Query["fail_url"].ToString();
                return Redirect(failUrl);
            }
        }
    }
}
