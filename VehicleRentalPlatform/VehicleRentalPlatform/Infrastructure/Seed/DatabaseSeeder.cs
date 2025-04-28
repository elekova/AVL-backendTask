using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using VehicleRentalPlatform.Domain;
using VehicleRentalPlatform.Infrastructure.Persistence;

namespace VehicleRentalPlatform.Infrastructure.Seed
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Vehicles.Any())
            {
                SeedVehicles();
            }

            if (!_context.Telemetries.Any())
            {
                SeedTelemetry();
            }

            if (!_context.Customers.Any())
            {
                SeedCustomers();
            }

            _context.SaveChanges();
        }

        private void SeedVehicles()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),  // Pretvara zaglavlja u mala slova
            };

            using var reader = new StreamReader("Infrastructure/Seed/vehicles.csv");
            using var csv = new CsvReader(reader, config);

            var vehicles = csv.GetRecords<Vehicle>().ToList();
            _context.Vehicles.AddRange(vehicles);
        }

        private void SeedTelemetry()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader("Infrastructure/Seed/telemetry.csv");
            using var csv = new CsvReader(reader, config);

            var telemetryRecords = csv.GetRecords<Telemetry>().ToList();

            var validTelemetryRecords = new List<Telemetry>();

            var earliestAllowedTimestamp = new DateTime(2020, 1, 1).ToUniversalTime();

            foreach (var telemetry in telemetryRecords)
            {
                
                if (telemetry.Timestamp <= 0)
                    continue;

                var telemetryDateTime = DateTimeOffset.FromUnixTimeSeconds(telemetry.Timestamp).UtcDateTime;
                if (telemetryDateTime < earliestAllowedTimestamp)
                    continue;

                
                if (telemetry.Value < 0)
                    continue;

                
                if (telemetry.Name == "battery_soc" && (telemetry.Value < 0 || telemetry.Value > 100))
                    continue;

                
                validTelemetryRecords.Add(telemetry);
            }

            
            _context.Telemetries.AddRange(validTelemetryRecords);
            _context.SaveChanges();
        }

        private void SeedCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "Ivan Horvat" },
                new Customer { Id = Guid.NewGuid(), Name = "Ana Kovač" }
            };

            _context.Customers.AddRange(customers);
        }



    }
}

