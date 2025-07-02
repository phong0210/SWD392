using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> GetSalesDashboard()
        {
            var query = new GetSalesDashboardQuery();
            var dashboard = await _mediator.Send(query);
            return Ok(dashboard);
        }
    }
} 