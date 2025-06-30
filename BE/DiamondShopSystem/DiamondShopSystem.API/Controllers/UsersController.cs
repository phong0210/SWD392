using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand { User = request };
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpGet("{id}")] // Assuming a GetUserById query exists or will be created
        [Authorize(Roles = "Customer,SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            // This would typically be a GetUserByIdQuery
            // For now, returning a placeholder or NotFound
            return NotFound();
        }

        [HttpPut("profile")]
        [Authorize(Roles = "Customer,SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] ManageUserAccountRequest request)
        {
            // This endpoint is for a user to update their OWN profile.
            // The UserId in the request should ideally come from the authenticated user's claims.
            // For simplicity, we're using ManageUserAccountRequest, but a dedicated DTO might be better.
            var command = new ManageUserAccountCommand { UserAccount = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/register-vip")]
        [Authorize(Roles = "SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> RegisterCustomerForVip(Guid id, [FromBody] RegisterCustomerForVipRequest request)
        {
            if (id != request.CustomerId)
            {
                return BadRequest("Customer ID in URL and body do not match.");
            }
            var command = new RegisterCustomerForVipCommand { Request = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Could not register customer for VIP.");
            }
            return NoContent();
        }

        [HttpPut("{id}/manage")]
        [Authorize(Roles = "HeadOfficeAdmin")]
        public async Task<IActionResult> ManageUserAccount(Guid id, [FromBody] ManageUserAccountRequest request)
        {
            if (id != request.UserId)
            {
                return BadRequest("User ID in URL and body do not match.");
            }
            var command = new ManageUserAccountCommand { UserAccount = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}