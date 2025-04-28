using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Domain;
using VehicleRentalPlatform.Infrastructure.Persistence;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET: api/customers/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _context.Customers
                .Include(c => c.Rentals)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

           
            double totalDistance = customer.Rentals
                .Where(r => r.State != "cancelled") 
                .Sum(r => Math.Max(0, r.EndOdometer - r.StartOdometer));

            decimal totalPrice = 0;

            foreach (var rental in customer.Rentals.Where(r => r.State != "cancelled"))
            {
                
                var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Vin == rental.VehicleVin);
                if (vehicle == null) continue;

                var rentalDays = (rental.EndTime.Date - rental.StartTime.Date).Days + 1;
                var batteryDelta = rental.EndBattery - rental.StartBattery;

                decimal price =
                    (Math.Max(0, (decimal)(rental.EndOdometer - rental.StartOdometer)) * vehicle.PricePerKmInEuro) +
                    ((decimal)rentalDays * vehicle.PricePerDayInEuro) +
                    (Math.Max(0, (decimal)-batteryDelta) * 0.2m);

                totalPrice += price;
            }

            var result = new
            {
                Id = customer.Id,
                Name = customer.Name,
                TotalDistanceKm = totalDistance,
                TotalPriceEuro = totalPrice
            };

            return Ok(result);
        }

        //  POST: api/customers
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Customer updatedCustomer)
        {
            
            if (id != updatedCustomer.Id)
            {
                return BadRequest("Customer ID mismatch");
            }

            
            _context.Entry(updatedCustomer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //  DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)  
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}