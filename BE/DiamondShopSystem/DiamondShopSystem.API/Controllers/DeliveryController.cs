using DiamondShopSystem.BLL.Models;
using DiamondShopSystem.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryById(Guid id)
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryDto createDeliveryDto)
        {
            var createdDelivery = await _deliveryService.CreateDeliveryAsync(createDeliveryDto);
            if (createdDelivery == null)
            {
                return Conflict("A delivery for this order already exists.");
            }
            return CreatedAtAction(nameof(GetDeliveryById), new { id = createdDelivery.Id }, createdDelivery);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery(Guid id, [FromBody] UpdateDeliveryDto updateDeliveryDto)
        {
            var result = await _deliveryService.UpdateDeliveryAsync(id, updateDeliveryDto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(Guid id)
        {
            var result = await _deliveryService.DeleteDeliveryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}