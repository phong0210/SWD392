using Microsoft.AspNetCore.Mvc;
using DiamondShopSystem.API.Policies;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [UseCorsPolicy(AuthorizationPolicies.AllowAll)]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test endpoint is working!");
        }
    }
} 