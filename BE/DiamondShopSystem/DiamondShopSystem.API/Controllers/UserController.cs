using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User;
using DiamondShopSystem.API.Policies;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(AuthorizationPolicies.UserEndpoints)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [EnableCors(AuthorizationPolicies.UserEndpoints)]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var result = await _mediator.Send(new UserCreateCommand(dto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
