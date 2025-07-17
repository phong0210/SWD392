using DiamondShopSystem.BLL.Handlers.Product.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Product.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Product.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Warranty.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Warranty.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.BLL.Services.Warranty;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWarrantyService _warrantyService;

        public WarrantyController(IMediator mediator, IWarrantyService warrantyService)
        {
            _mediator = mediator;
            _warrantyService = warrantyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var warranties = await _warrantyService.GetAllWarrantiesAsync();
            return Ok(warranties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new WarrantyGetCommand(id));
                if (!result.Success)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WarrantyCreateDto warranty)
        {
            var result = await _mediator.Send(new WarrantyCreateCommand(warranty));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WarrantyUpdateDto update)
        {
            try
            {
                var result = await _mediator.Send(new WarrantyUpdateCommand(id, update));
                if (!result.Success)
                    return BadRequest(new { success = false, error = result.Error });
                return Ok(new { success = true, dto = result.Warranty });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _warrantyService.DeleteWarrantyAsync(id); // No need to assign the result since the method returns void.
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
}