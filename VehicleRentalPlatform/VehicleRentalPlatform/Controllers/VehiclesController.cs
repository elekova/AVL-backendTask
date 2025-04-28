using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Infrastructure.Persistence;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        //// GET: api/vehicles/{vin}
        [HttpGet("{vin}")]
        public async Task<IActionResult> GetByVin(string vin)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Rentals)  
                .FirstOrDefaultAsync(v => v.Vin == vin);  

            if (vehicle == null)
            {
                return NotFound();
            }

            
            double totalDistanceDriven = vehicle.Rentals.Sum(r => r.EndOdometer - r.StartOdometer);

            
            int totalRentalCount = vehicle.Rentals.Count;

            
            decimal totalRentalIncome = vehicle.Rentals.Sum(rental =>
            {
                var pricePerKm = vehicle.PricePerKmInEuro;
                var pricePerDay = vehicle.PricePerDayInEuro;
                var rentalDays = (rental.EndTime - rental.StartTime).Days;

                double distanceDriven = rental.EndOdometer - rental.StartOdometer;
                decimal income = ((decimal)distanceDriven * pricePerKm) + (rentalDays * pricePerDay);

                return income;
            });

            var vehicleDetails = new
            {
                vehicle.Vin,
                TotalDistanceDriven = totalDistanceDriven,
                TotalRentalCount = totalRentalCount,
                TotalRentalIncome = totalRentalIncome
            };

            return Ok(vehicleDetails);
        }


    }
}
