using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var query = new LoginQuery 
            { 
                Login = request,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return Ok(response);
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var query = new GoogleLoginQuery { IdToken = idToken };
            var response = await _mediator.Send(query);
            if (response == null)
            {
                return Unauthorized("Invalid Google token or email not verified.");
            }
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var command = new ForgotPasswordCommand 
            { 
                Request = request,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var command = new ResetPasswordCommand 
            { 
                Request = request,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };
            var response = await _mediator.Send(command);
            
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            
            return Ok(response);
        }
    }
}