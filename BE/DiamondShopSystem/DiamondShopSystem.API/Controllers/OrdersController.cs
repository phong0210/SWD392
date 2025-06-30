using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DiamondShopSystem.BLL.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {
            var command = new PlaceOrderCommand 
            { 
                CustomerId = request.CustomerId,
                Items = request.Items,
                ShippingAddress = request.ShippingAddress
            };
            var order = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderStatus), new { id = order.Id }, order);
        }

        [HttpPost("cart/add")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // TODO: Get CustomerId from authenticated user's claims
            var customerId = Guid.Empty; // This should come from User.Claims
            
            var command = new AddToCartCommand
            {
                CustomerId = customerId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            var cartItem = await _mediator.Send(command);
            return Ok(cartItem);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> GetOrderStatus(Guid id)
        {
            var query = new GetOrderStatusQuery { OrderId = id };
            var order = await _mediator.Send(query);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderHistory(Guid customerId)
        {
            var query = new GetOrderHistoryQuery { CustomerId = customerId };
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            if (id != request.OrderId)
            {
                return BadRequest("Order ID in URL and body do not match.");
            }
            if (!Enum.TryParse<OrderStatus>(request.NewStatus, true, out var status))
            {
                return BadRequest($"Invalid status: {request.NewStatus}");
            }
            var command = new UpdateRequestStatusCommand
            {
                OrderId = request.OrderId,
                StaffId = Guid.Empty, // TODO: Replace with actual staff ID from auth context
                NewStatus = status,
                Notes = string.Empty
            };
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var command = new ConfirmOrderCommand { OrderId = id };
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Could not confirm order.");
            }
            return Ok(result);
        }

        [HttpPost("{id}/fail")]
        [Authorize(Roles = "SalesStaff,StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> HandleOrderFailure(Guid id, [FromQuery] string reason)
        {
            var command = new HandleOrderFailureCommand { OrderId = id, Reason = reason };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest("Could not handle order failure.");
            }
            return NoContent();
        }
    }
}