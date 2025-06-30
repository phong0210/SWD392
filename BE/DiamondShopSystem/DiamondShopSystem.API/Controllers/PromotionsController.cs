using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Promotions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PromotionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "HeadOfficeAdmin")]
        public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionRequest request)
        {
            var command = new CreatePromotionCommand
            {
                Code = request.Code,
                Description = request.Description,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            var promotion = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPromotionById), new { id = promotion.Id }, promotion);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPromotionById(Guid id)
        {
            // This would typically be a GetPromotionByIdQuery
            // For now, returning a placeholder
            return NotFound();
        }
    }
} 