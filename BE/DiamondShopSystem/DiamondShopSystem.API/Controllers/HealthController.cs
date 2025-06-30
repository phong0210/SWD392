using DiamondShopSystem.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly DiamondShopDbContext _context;

    public HealthController(DiamondShopDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Test database connection
            await _context.Database.CanConnectAsync();

            return Ok(new
            {
                Status = "Healthy",
                Database = "Connected",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Unhealthy",
                Database = "Connection Failed",
                Error = ex.Message,
                InnerException = ex.InnerException?.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("db-info")]
    public IActionResult GetDatabaseInfo()
    {
        try
        {
            var connectionString = _context.Database.GetConnectionString();

            // Parse connection string to show info (hide password)
            var parts = connectionString?.Split(';') ?? new string[0];
            var info = new Dictionary<string, string>();

            foreach (var part in parts)
            {
                if (part.Contains('='))
                {
                    var keyValue = part.Split('=', 2);
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();

                    // Hide sensitive information
                    if (key.ToLower().Contains("password"))
                        value = "***";

                    info[key] = value;
                }
            }

            return Ok(new
            {
                ConnectionInfo = info,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}