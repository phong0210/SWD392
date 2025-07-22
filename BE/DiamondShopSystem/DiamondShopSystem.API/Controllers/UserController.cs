using Microsoft.AspNetCore.Mvc;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Register;
using DiamondShopSystem.BLL.Handlers.User.Commands.Get;
using DiamondShopSystem.BLL.Handlers.User.Commands.Update;
using DiamondShopSystem.API.Policies;
using DiamondShopSystem.BLL.Handlers.User.Commands.ConfirmRegistration;
using DiamondShopSystem.BLL.Handlers.User.Commands.PromoteUserToStaff;


namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UseCorsPolicy(AuthorizationPolicies.UserEndpoints)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new UserGetCommand(id));
            
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserListDto>>> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var result = await _mediator.Send(new UserRegisterCommand(userRegisterDto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("confirm-registration")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationDto confirmRegistrationDto)
        {
            var result = await _mediator.Send(new ConfirmRegistrationCommand(confirmRegistrationDto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var result = await _mediator.Send(new UserUpdateCommand(id, userUpdateDto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("promote-to-staff")]
        public async Task<IActionResult> PromoteToStaff([FromBody] PromoteUserToStaffCommand command)
        {
            var success = await _mediator.Send(command);
            if (!success)
                return BadRequest(new { message = "Failed to promote user to staff." });
            return Ok(new { message = "User promoted to staff successfully." });
        }
    }
}
