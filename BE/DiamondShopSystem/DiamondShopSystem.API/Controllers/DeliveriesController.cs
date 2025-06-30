using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Deliveries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "DeliveryStaff")]
        public async Task<IActionResult> GetAssignedDeliveries([FromQuery] Guid deliveryStaffId)
        {
            var query = new GetAssignedDeliveriesQuery { DeliveryStaffId = deliveryStaffId };
            var deliveries = await _mediator.Send(query);
            return Ok(deliveries);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "DeliveryStaff")]
        public async Task<IActionResult> UpdateShipmentStatus(Guid id, [FromBody] UpdateShipmentStatusRequest request)
        {
            if (id != request.DeliveryId)
            {
                return BadRequest("Delivery ID in URL and body do not match.");
            }
            var command = new UpdateShipmentStatusCommand { Request = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
} 