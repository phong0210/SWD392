
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Order.Commands.Delete;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetAll;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetById;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRelations;
using DiamondShopSystem.BLL.Handlers.Order.Queries.GetRevenueSummary;
using DiamondShopSystem.BLL.Handlers.Order.DTOs;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var result = await _mediator.Send(new OrderCreateCommand(createOrderDto));
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPost("summarize")]
        public async Task<ActionResult<GetRevenueSummaryResponseDto>> GetRevenueSummary()
        {
            try
            {
                var result = await _mediator.Send(new GetRevenueSummaryQuery());
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                var result = await _mediator.Send(new OrderUpdateCommand(id, updateOrderDto));
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteOrderCommand(id));
                if (!result.Success)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetAllOrders()
        {
            try
            {
                var orders = await _mediator.Send(new GetAllOrdersQuery());
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderByIdResponseDto>> GetOrderById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetOrderByIdQuery(id));
                if (!result.Success)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("{id}/user-info")]
        public async Task<ActionResult<GetOrderRelationsResponseDto>> GetOrderRelations(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetOrderRelationsQuery(id));
                if (!result.Success)
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    }
}
