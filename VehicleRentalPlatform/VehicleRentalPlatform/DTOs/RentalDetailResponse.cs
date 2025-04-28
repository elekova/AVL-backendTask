
namespace VehicleRentalPlatform.DTOs
{
    public class RentalDetailsResponse
    {
        public int Id { get; set; }
        public string VehicleVin { get; set; }
        public Guid CustomerId { get; set; }
        public double DistanceKm { get; set; }
        public double StartBattery { get; set; }
        public double EndBattery { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string State { get; set; }
    }
}
