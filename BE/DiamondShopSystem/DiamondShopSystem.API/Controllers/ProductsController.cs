using MediatR;
using DiamondShopSystem.BLL.Application.DTOs;
using DiamondShopSystem.BLL.Application.Features.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCatalog(
            [FromQuery] string searchTerm, 
            [FromQuery] Guid? categoryId, 
            [FromQuery] decimal? minPrice, 
            [FromQuery] decimal? maxPrice, 
            [FromQuery] string color, 
            [FromQuery] string clarity, 
            [FromQuery] string cut, 
            [FromQuery] decimal? minCarat, 
            [FromQuery] decimal? maxCarat, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProductCatalogQuery
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Color = color,
                Clarity = clarity,
                Cut = cut,
                MinCarat = minCarat,
                MaxCarat = maxCarat,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductDetails(Guid id)
        {
            var query = new GetProductDetailsQuery { Id = id };
            var product = await _mediator.Send(query);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto request)
        {
            var command = new CreateProductCommand { Product = request };
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductDetails), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto request)
        {
            if (id != request.Id)
            {
                return BadRequest("Product ID in URL and body do not match.");
            }
            var command = new UpdateProductCommand { Product = request };
            var product = await _mediator.Send(command);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}/hide")]
        [Authorize(Roles = "StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> HideProduct(Guid id, [FromQuery] bool hide = true)
        {
            var command = new HideProductCommand { Id = id, Hide = hide };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}/inventory")]
        [Authorize(Roles = "StoreManager,HeadOfficeAdmin")]
        public async Task<IActionResult> UpdateInventory(Guid id, [FromBody] UpdateInventoryRequest request)
        {
            if (id != request.ProductId)
            {
                return BadRequest("Product ID in URL and body do not match.");
            }
            var command = new UpdateInventoryCommand { Request = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}/pricing")]
        [Authorize(Roles = "HeadOfficeAdmin")]
        public async Task<IActionResult> UpdatePricingParameters(Guid id, [FromBody] UpdatePricingParametersRequest request)
        {
            var command = new UpdatePricingParametersCommand { Request = request };
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
