using DiamondShopSystem.API.Policies;
using DiamondShopSystem.BLL.Handlers.Product.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Product.Commands.Get;
using DiamondShopSystem.BLL.Handlers.Product.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Product.DTOs;
using DiamondShopSystem.BLL.Handlers.User.Commands.Update;
using DiamondShopSystem.BLL.Handlers.User.DTOs;
using DiamondShopSystem.BLL.Services.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UseCorsPolicy(AuthorizationPolicies.UserEndpoints)]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductService _productService;

        public ProductController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new ProductGetCommand(id));
                if (!result.Success)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto updateDto)
        {
            try
            {
                var result = await _mediator.Send(new ProductUpdateCommand(id, updateDto));
                if (!result.Success)
                    return BadRequest(new { success = false, error = result.Error });
                return Ok(new { success = true, product = result.Product });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var result = await _mediator.Send(new ProductCreateCommand(dto));
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,Boolean status)
        {
            try
            {
                await _productService.DeleteProductAsync(id,status); // No need to assign the result since the method returns void.
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

    }
}