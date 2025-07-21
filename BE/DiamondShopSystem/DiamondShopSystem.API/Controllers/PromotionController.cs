
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Promotion.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Promotion.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Promotion.Commands.Delete;
using DiamondShopSystem.BLL.Handlers.Promotion.Queries;
using DiamondShopSystem.BLL.Handlers.Promotion.DTOs;
using System;
using System.Collections.Generic;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PromotionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionCreateDto promotionDto)
        {
            var command = new PromotionCreateCommand(promotionDto);
            var promotion = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPromotionById), new { promotionId = promotion.Id }, promotion);
        }

        [HttpGet("GetPromotionById/{promotionId}")]
        public async Task<IActionResult> GetPromotionById(Guid promotionId)
        {
            var query = new GetPromotionByIdQuery { PromotionId = promotionId };
            var promotion = await _mediator.Send(query);
            if (promotion == null) return NotFound();
            return Ok(promotion);
        }

        [HttpGet("ShowAll")]
        public async Task<ActionResult<IEnumerable<PromotionDto>>> GetAllPromotions()
        {
            var query = new GetAllPromotionsQuery();
            var promotions = await _mediator.Send(query);
            return Ok(promotions);
        }

        [HttpPut("UpdatePromotion/{id}")]
        public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] PromotionUpdateDto promotionDto)
        {
            var command = new UpdatePromotionCommand { Id = id, Dto = promotionDto };
            var promotion = await _mediator.Send(command);
            if (promotion == null) return NotFound();
            return Ok(promotion);
        }

        [HttpDelete("DeletePromotion/{id}")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            var command = new DeletePromotionCommand { Id = id };
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
