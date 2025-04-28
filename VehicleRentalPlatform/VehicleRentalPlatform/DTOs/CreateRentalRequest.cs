namespace VehicleRentalPlatform.DTOs
{
    public class CreateRentalRequest
    {
        public string VehicleVin { get; set; }
        public Guid CustomerId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string State { get; set; }

        public double StartOdometer { get; set; }
        public double EndOdometer { get; set; }
        public double StartBattery { get; set; }
        public double EndBattery { get; set; }
    }
}

