using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MediatR;
using DiamondShopSystem.BLL.Handlers.Auth;
using DiamondShopSystem.API.Policies;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(AuthorizationPolicies.AuthEndpoints)]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [EnableCors(AuthorizationPolicies.AuthEndpoints)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _mediator.Send(new LoginCommand(request));
            if (result == null)
                return Unauthorized(new { error = "Invalid credentials" });
            return Ok(result);
        }
    }
} 