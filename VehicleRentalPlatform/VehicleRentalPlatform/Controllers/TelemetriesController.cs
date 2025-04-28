using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Infrastructure.Persistence;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TelemetriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTelemetries([FromQuery] double value)
        {
            var query = _context.Telemetries.AsQueryable();


            query = query.Where(t => t.Value == value);
            

            var telemetries = await query.ToListAsync();
            return Ok(telemetries);
        }
    }
}
