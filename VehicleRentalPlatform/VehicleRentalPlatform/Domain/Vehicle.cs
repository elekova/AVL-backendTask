namespace VehicleRentalPlatform.Domain
{
    public class Vehicle
    {
        public string Vin { get; set; } // Primary Key
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal PricePerKmInEuro { get; set; }
        public decimal PricePerDayInEuro { get; set; }

        // Navigation Properties
        public ICollection<Telemetry> Telemetries { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
