using Microsoft.AspNetCore.Mvc;
using MediatR;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Delete;
using DiamondShopSystem.API.Policies;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UseCorsPolicy(AuthorizationPolicies.UserEndpoints)]
    public class OrderDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetOrderDetailById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailGetCommand(id));
                
                if (!result.Success)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetAllOrderDetail")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailGetAllCommand());
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("GetOrderDetailByOrderId/{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailGetByOrderCommand(orderId));
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("CreateOrderDetail")]
        public async Task<IActionResult> Create([FromBody] OrderDetailCreateDto orderDetailCreateDto)
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailCreateCommand(orderDetailCreateDto));
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPut("UpdateOrderDetail/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDetailUpdateDto orderDetailUpdateDto)
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailUpdateCommand(id, orderDetailUpdateDto));
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("DeleteOrderDetail/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new OrderDetailDeleteCommand(id));
                if (!result.Success)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
} 