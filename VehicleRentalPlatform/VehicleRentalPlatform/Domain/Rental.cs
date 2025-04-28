namespace VehicleRentalPlatform.Domain
{
    public class Rental
    {
        public int Id { get; set; } // Primary Key
        public string VehicleVin { get; set; }
        public Guid CustomerId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string State { get; set; } // "ordered" ,"cancelled"

        public double StartOdometer { get; set; }
        public double EndOdometer { get; set; }
        public double StartBattery { get; set; }
        public double EndBattery { get; set; }

        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
    }
}
