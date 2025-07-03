 using Microsoft.AspNetCore.Mvc;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var result = await _mediator.Send(new UserCreateCommand(dto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
