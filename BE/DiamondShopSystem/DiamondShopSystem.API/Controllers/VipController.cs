using DiamondShopSystem.BLL.Handlers.Vip.DTOs;
using DiamondShopSystem.BLL.Interfaces;
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
            var response = await _vipService.GetAllVipsAsync();
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return Ok(response.Vips);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VipDto>> GetVipById(Guid id)
        {
            var response = await _vipService.GetVipByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response.Error);
            }
            return Ok(response.Vip);
        }

        [HttpPost]
        public async Task<ActionResult<VipDto>> CreateVip([FromBody] VipCreateRequestDto request)
        {
            var response = await _vipService.CreateVipAsync(request);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }
            return CreatedAtAction(nameof(GetVipById), new { id = response.Vip?.VipId }, response.Vip);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVip(Guid id, [FromBody] VipUpdateRequestDto request)
        {
            var response = await _vipService.UpdateVipAsync(id, request);
            if (!response.Success)
            {
                return NotFound(response.Error);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVip(Guid id)
        {
            var response = await _vipService.DeleteVipAsync(id);
            if (!response.Success)
            {
                return NotFound(response.Error);
            }
            return NoContent();
        }
    }
}