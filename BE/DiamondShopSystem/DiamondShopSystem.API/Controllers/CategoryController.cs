using DiamondShopSystem.BLL.Handlers.Category.DTOs;
using DiamondShopSystem.BLL.Handlers.Category.Commands.Create;
using DiamondShopSystem.BLL.Handlers.Category.Commands.Update;
using DiamondShopSystem.BLL.Handlers.Category.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DiamondShopSystem.BLL.Handlers.Category.Commands.Get;
using DiamondShopSystem.BLL.Services.Category;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMediator mediator, ICategoryService categoryService)
        {
            _mediator = mediator;
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
             try
            {
                var result = await _mediator.Send(new CategoryGetCommand(id));
                if (!result.Success)
                    return NotFound(result);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var result = await _mediator.Send(new CategoryCreateCommand(dto));
            if (result == null)
                return BadRequest(new { success = false, error = "Create failed" });
            return Ok(new { success = true, category = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var result = await _mediator.Send(new CategoryUpdateCommand(id, dto));
            if (result == null)
                return BadRequest(new { success = false, error = "Update failed" });
            return Ok(new { success = true, category = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new CategoryDeleteCommand(id));
            if (!result)
                return NotFound(new { success = false, error = "Category not found" });
            return Ok(new { success = true });
        }
    }
}   