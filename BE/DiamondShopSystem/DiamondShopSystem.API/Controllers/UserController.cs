using Microsoft.AspNetCore.Mvc;
using MediatR;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Create;
using DiamondShopSystem.BLL.Handlers.User.Commands.Get;
using DiamondShopSystem.BLL.Handlers.User.Commands.Update;
using DiamondShopSystem.API.Policies;
using System;
using System.Threading.Tasks;
using FluentValidation;

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
            try
            {
                var result = await _mediator.Send(new UserGetCommand(id));
                
                if (!result.Success)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
        {
            try
            {
                var result = await _mediator.Send(new UserCreateCommand(userCreateDto));
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                var result = await _mediator.Send(new UserUpdateCommand(id, userUpdateDto));
                if (!result.Success)
                    return BadRequest(new { success = false, error = result.Error });
                return Ok(new { success = true, user = result.User });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
}
