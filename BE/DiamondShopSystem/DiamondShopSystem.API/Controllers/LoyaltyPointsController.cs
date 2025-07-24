using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Create;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Delete;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.Update;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Commands.UpdateByUserId;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.Queries.GetById;
using DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoyaltyPointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoyaltyPointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLoyaltyPoint([FromBody] LoyaltyPointCreateDto loyaltyPointDto)
        {
            var command = new LoyaltyPointCreateCommand(loyaltyPointDto);
            var loyaltyPoint = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetLoyaltyPointById), new { loyaltyPointId = loyaltyPoint.Id }, loyaltyPoint);
        }

        [HttpGet("GetLoyaltyPointById/{loyaltyPointId}")]
        public async Task<IActionResult> GetLoyaltyPointById(Guid loyaltyPointId)
        {
            var query = new GetLoyaltyPointByIdQuery { LoyaltyPointId = loyaltyPointId };
            var loyaltyPoint = await _mediator.Send(query);
            if (loyaltyPoint == null) return NotFound();
            return Ok(loyaltyPoint);
        }

        [HttpGet("GetLoyaltyPointByUserId/{userId}")]
        public async Task<IActionResult> GetLoyaltyPointByUserId(Guid userId)
        {
            var query = new GetLoyaltyPointByUserIdQuery(userId);
            var loyaltyPoint = await _mediator.Send(query);
            if (loyaltyPoint == null) return NotFound();
            return Ok(loyaltyPoint);
        }

        [HttpPut("user/{userId}/loyalty-points")]
        public async Task<ActionResult<LoyaltyPointDto>> UpdateLoyaltyPointsByUserId(Guid userId,LoyaltyPointUpdateDto updateDto)
        {
            var command = new UpdateLoyaltyPointByUserIdCommand
            {
                UserId = userId,
                Dto = updateDto
            };

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"Loyalty points not found for user {userId}");

            return Ok(result);
        }

        [HttpGet("ShowAll")]
        public async Task<ActionResult<IEnumerable<LoyaltyPointDto>>> GetAllLoyaltyPoints()
        {
            var query = new GetAllLoyaltyPointsQuery();
            var loyaltyPoints = await _mediator.Send(query);
            return Ok(loyaltyPoints);
        }

        [HttpPut("UpdateLoyaltyPoint/{id}")]
        public async Task<IActionResult> UpdateLoyaltyPoint(Guid id, [FromBody] LoyaltyPointUpdateDto loyaltyPointDto)
        {
            var command = new UpdateLoyaltyPointCommand { Id = id, Dto = loyaltyPointDto };
            var loyaltyPoint = await _mediator.Send(command);
            if (loyaltyPoint == null) return NotFound();
            return Ok(loyaltyPoint);
        }

        [HttpDelete("DeleteLoyaltyPoint/{id}")]
        public async Task<IActionResult> DeleteLoyaltyPoint(Guid id)
        {
            var command = new DeleteLoyaltyPointCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}