using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Domain;
using VehicleRentalPlatform.DTOs;
using VehicleRentalPlatform.Infrastructure.Persistence;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        //  GET: api/rentals
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentals = await _context.Rentals.ToListAsync();
            return Ok(rentals);
        }

        //  GET: api/rentals/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rental = await _context.Rentals
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            
            double distanceKm = rental.EndOdometer - rental.StartOdometer;
            if (distanceKm < 0)
            {
                distanceKm = 0; 
            }

            var response = new RentalDetailsResponse
            {
                Id = rental.Id,
                VehicleVin = rental.VehicleVin,
                CustomerId = rental.CustomerId,
                DistanceKm = distanceKm,
                StartBattery = rental.StartBattery,
                EndBattery = rental.EndBattery,
                StartTime = rental.StartTime,
                EndTime = rental.EndTime,
                State = rental.State
            };

            return Ok(response);
        }

        //  POST: api/rentals
        [HttpPost]
        public async Task<IActionResult> Create(CreateRentalRequest rentalDto)
        {
            
            bool overlapExists = await _context.Rentals
                .AnyAsync(r => r.VehicleVin == rentalDto.VehicleVin
                            && r.CustomerId == rentalDto.CustomerId
                            && r.State != "cancelled"
                            && (
                                (rentalDto.StartTime >= r.StartTime && rentalDto.StartTime <= r.EndTime) ||
                                (rentalDto.EndTime >= r.StartTime && rentalDto.EndTime <= r.EndTime) ||
                                (rentalDto.StartTime <= r.StartTime && rentalDto.EndTime >= r.EndTime)
                            )
                );

            if (overlapExists)
            {
                return BadRequest("Overlapping rental exists for the same vehicle and customer.");
            }

            
            var rental = new Rental
            {
                VehicleVin = rentalDto.VehicleVin,
                CustomerId = rentalDto.CustomerId,
                StartTime = rentalDto.StartTime,
                EndTime = rentalDto.EndTime,
                State = "ordered", 
                StartOdometer = rentalDto.StartOdometer,
                EndOdometer = rentalDto.EndOdometer,
                StartBattery = rentalDto.StartBattery,
                EndBattery = rentalDto.EndBattery
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }



        // PUT: api/rentals/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRentalRequest request)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            
            rental.StartTime = request.StartTime;
            rental.EndTime = request.EndTime;
            rental.State = request.State;
            rental.StartOdometer = request.StartOdometer;
            rental.EndOdometer = request.EndOdometer;
            rental.StartBattery = request.StartBattery;
            rental.EndBattery = request.EndBattery;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //  DELETE: api/rentals/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound();

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
