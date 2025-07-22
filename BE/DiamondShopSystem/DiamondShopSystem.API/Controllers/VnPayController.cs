using DiamondShopSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromQuery] Guid orderId, [FromQuery] decimal amount)
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    return BadRequest("Could not determine client IP address.");
                }

                var paymentUrl = _vnPayService.CreatePaymentUrl(orderId, amount, ipAddress);
                return Ok(new { PaymentUrl = paymentUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("payment-callback")]
        public IActionResult VnPayCallback()
        {
            try
            {
                var result = _vnPayService.ProcessPaymentCallback(HttpContext.Request.Query);
                if (result.IsSuccess)
                {
                    // Handle successful payment, e.g., update order status in your database
                    return Ok(result);
                }
                else
                {
                    // Handle failed payment
                    return BadRequest(result);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
