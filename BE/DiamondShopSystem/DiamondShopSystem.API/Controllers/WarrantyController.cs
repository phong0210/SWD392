using DiamondShopSystem.BLL.Handlers.Warranty.DTOs;
using DiamondShopSystem.BLL.Services.Warranty;
using DiamondShopSystem.DAL;
using DiamondShopSystem.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiamondShopSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWarrantyService _warrantyService;

        public WarrantyController(AppDbContext context, IWarrantyService warrantyService)
        {
            _context = context;
            _warrantyService = warrantyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warranty>>> GetAll()
        {
            var warranties = await _warrantyService.GetAllWarrantiesAsync();
            return Ok(warranties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Warranty>> GetById(Guid id)
        {
            var warranty = await _warrantyService.GetWarrantyByIdAsync(id);
            if (warranty == null)
                return NotFound(new { success = false, error = "Warranty not found" });
            return Ok(warranty);
        }

        [HttpPost]
        public async Task<ActionResult<Warranty>> Create([FromBody] Warranty warranty)
        {
            warranty.Id = Guid.NewGuid();
            _context.Warranties.Add(warranty);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, warranty });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WarrantyUpdateDto update)
        {
            var warranty = await _warrantyService.UpdateWarrantyAsync(id, update);
            if (warranty == null)
                return NotFound(new { success = false, error = "Warranty not found" });
            return Ok(new { success = true, warranty });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var warranty = await _context.Warranties.FindAsync(id);
            if (warranty == null)
                return NotFound(new { success = false, error = "Warranty not found" });

            _context.Warranties.Remove(warranty);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}