using Microsoft.AspNetCore.Mvc;
using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth.DTOs;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.Login;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.SendOtp;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.VerifyOtp;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.RequestPasswordReset;
using DiamondShopSystem.BLL.Handlers.Auth.Commands.ConfirmPasswordReset;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _mediator.Send(new LoginCommand(request));
            if (!string.IsNullOrEmpty(result.Error))
            {
                return Unauthorized(new { message = result.Error });
            }
            return Ok(result);
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request)
        {
            await _mediator.Send(new SendOtpCommand(request));
            return Ok(new { message = "If an account with this email exists, an OTP has been sent." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
        {
            var isVerified = await _mediator.Send(new VerifyOtpCommand(request));
            if (isVerified)
            {
                return Ok(new { message = "OTP verified successfully." });
            }
            return BadRequest(new { message = "Invalid or expired OTP." });
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto request)
        {
            var result = await _mediator.Send(new RequestPasswordResetCommand(request));
            if (result)
            {
                return Ok(new { message = "If an account with this email exists, an OTP has been sent to reset your password." });
            }
            return BadRequest(new { message = "Failed to send OTP. Please check your email." });
        }

        [HttpPost("confirm-password-reset")]
        public async Task<IActionResult> ConfirmPasswordReset([FromBody] ConfirmPasswordResetDto request)
        {
            var result = await _mediator.Send(new ConfirmPasswordResetCommand(request));
            if (result)
            {
                return Ok(new { message = "Password has been reset successfully." });
            }
            return BadRequest(new { message = "Invalid OTP or email, or password reset failed." });
        }
    }
} 