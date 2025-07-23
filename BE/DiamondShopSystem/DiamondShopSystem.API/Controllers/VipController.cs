using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Services.Vip;
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
        private readonly IVipService _vipService;

        public VipController(IVipService vipService)
        {
            _vipService = vipService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VipDto>>> GetAllVips()
        {
            var vips = await _vipService.GetAllVipsAsync();
            return Ok(vips);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VipDto>> GetVipById(Guid id)
        {
            var vip = await _vipService.GetVipByIdAsync(id);
            if (vip == null)
            {
                return NotFound();
            }
            return Ok(vip);
        }

        [HttpPost]
        public async Task<ActionResult<VipDto>> CreateVip([FromBody] VipCreateRequestDto request)
        {
            var vip = await _vipService.CreateVipAsync(request);
            return CreatedAtAction(nameof(GetVipById), new { id = vip.VipId }, vip);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVip(Guid id, [FromBody] VipUpdateRequestDto request)
        {
            var vip = await _vipService.UpdateVipAsync(id, request);
            if (vip == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVip(Guid id)
        {
            var result = await _vipService.DeleteVipAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}