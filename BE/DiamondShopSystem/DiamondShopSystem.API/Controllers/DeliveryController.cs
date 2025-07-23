using DiamondShopSystem.BLL.Handlers.Delivery.DTOs;
using DiamondShopSystem.BLL.Services.Delivery;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
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
            return CreatedAtAction(nameof(GetDeliveryById), new { id = createdDelivery.OrderId }, createdDelivery);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery(Guid id, [FromBody] UpdateDeliveryDto updateDeliveryDto)
        {
            await _deliveryService.UpdateDeliveryAsync(id, updateDeliveryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(Guid id)
        {
            await _deliveryService.DeleteDeliveryAsync(id);
            return NoContent();
        }
    }
}