using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("vnpay/create")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateVNPayPayment([FromBody] CreateVNPayPaymentRequest request)
        {
            // This would involve a command to generate VNPay URL
            // For now, this is a placeholder.
            return StatusCode(501, "VNPay payment creation not yet implemented.");
        }

        [HttpGet("vnpay/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> VNPayCallback()
        {
            // This would involve a query/command to handle VNPay IPN callback
            // For now, this is a placeholder.
            return StatusCode(501, "VNPay callback handling not yet implemented.");
        }
    }
}