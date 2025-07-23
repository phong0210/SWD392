using Microsoft.AspNetCore.Mvc;
using DiamondShopSystem.BLL.Services.LoyaltyPoint;
using DiamondShopSystem.BLL.Handlers.LoyaltyPoints.DTOs;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyPointsController : ControllerBase
    {
        private readonly ILoyaltyPointService _loyaltyPointService;

        public LoyaltyPointsController(ILoyaltyPointService loyaltyPointService)
        {
            _loyaltyPointService = loyaltyPointService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllLoyaltyPoints()
        {
            try
            {
                var result = await _loyaltyPointService.GetAllLoyaltyPointsAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoyaltyPointById(Guid id)
        {
            try
            {
                var result = await _loyaltyPointService.GetLoyaltyPointByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, message = "Loyalty point not found" });
                }
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoyaltyPoint([FromBody] LoyaltyPointResponseDto loyaltyPointDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                await _loyaltyPointService.AddLoyaltyPointAsync(loyaltyPointDto);
                return CreatedAtAction(
                    nameof(GetLoyaltyPointById),
                    new { id = loyaltyPointDto.Id },
                    new { success = true, message = "Loyalty point created successfully" }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoyaltyPoint(Guid id, [FromBody] LoyaltyPointResponseDto loyaltyPointDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data", errors = ModelState });
                }

                if (id != loyaltyPointDto.Id)
                {
                    return BadRequest(new { success = false, message = "ID mismatch" });
                }

                var existingLoyaltyPoint = await _loyaltyPointService.GetLoyaltyPointByIdAsync(id);
                if (existingLoyaltyPoint == null)
                {
                    return NotFound(new { success = false, message = "Loyalty point not found" });
                }

                await _loyaltyPointService.UpdateLoyaltyPointAsync(loyaltyPointDto);
                return Ok(new { success = true, message = "Loyalty point updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoyaltyPoint(Guid id)
        {
            try
            {
                var existingLoyaltyPoint = await _loyaltyPointService.GetLoyaltyPointByIdAsync(id);
                if (existingLoyaltyPoint == null)
                {
                    return NotFound(new { success = false, message = "Loyalty point not found" });
                }

                await _loyaltyPointService.DeleteLoyaltyPointAsync(id);
                return Ok(new { success = true, message = "Loyalty point deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}