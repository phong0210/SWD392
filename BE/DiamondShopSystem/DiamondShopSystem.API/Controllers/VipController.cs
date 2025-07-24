using DiamondShopSystem.BLL.Handlers.Vip.Commands;
using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Handlers.Vip.Queries;
using DiamondShopSystem.BLL.Handlers.Vip.Queries.GetAll;
using DiamondShopSystem.BLL.Handlers.Vip.Queries.GetById;
using DiamondShopSystem.BLL.Handlers.Vip.Queries.GetByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateVip([FromBody] VipCreateRequestDto request)
        {
            var command = new CreateVipCommand(request);
            var vip = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetVipById), new { vipId = vip.VipId }, vip);
        }

        [HttpGet("GetVipById/{vipId}")]
        public async Task<IActionResult> GetVipById(Guid vipId)
        {
            var query = new GetVipByIdQuery(vipId);
            var vip = await _mediator.Send(query);
            if (vip == null) return NotFound();
            return Ok(vip);
        }

        [HttpGet("GetVipByUserId/{userId}")]
        public async Task<IActionResult> GetVipByUserId(Guid userId)
        {
            var query = new GetVipByUserIdQuery(userId);
            var vip = await _mediator.Send(query);
            if (vip == null) return NotFound();
            return Ok(vip);
        }

        [HttpGet("ShowAll")]
        public async Task<ActionResult<IEnumerable<VipDto>>> GetAllVips()
        {
            var query = new GetAllVipsQuery();
            var vips = await _mediator.Send(query);
            return Ok(vips);
        }

        [HttpPut("UpdateVip/{id}")]
        public async Task<IActionResult> UpdateVip(Guid id, [FromBody] VipUpdateRequestDto request)
        {
            var command = new UpdateVipCommand(id, request);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("DeleteVip/{id}")]
        public async Task<IActionResult> DeleteVip(Guid id)
        {
            var command = new DeleteVipCommand(id);
            var result = await _mediator.Send(command);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}