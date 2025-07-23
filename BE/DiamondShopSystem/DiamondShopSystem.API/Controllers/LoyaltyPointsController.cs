using Microsoft.AspNetCore.Mvc;
using DiamondShopSystem.BLL.Services.LoyaltyPoint;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyPointsController : ControllerBase
    {
        private readonly LoyaltyPointService _loyaltyPointService;

        public LoyaltyPointsController(LoyaltyPointService loyaltyPointService)
        {
            _loyaltyPointService = loyaltyPointService;
        }


        [HttpGet("{id}/loyalty-points")]
        public async Task<IActionResult> GetLoyaltyPoints(Guid id)
        {
            var result = await _loyaltyPointService.GetLoyaltyPointByIdAsync(id);
            return Ok(result);
        }
    }
}
