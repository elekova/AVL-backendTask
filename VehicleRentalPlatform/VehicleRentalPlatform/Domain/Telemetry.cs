using System.ComponentModel.DataAnnotations;

namespace VehicleRentalPlatform.Domain
{
    public class Telemetry
    {
        [Key]
        public int Id { get; set; }
        public string Vin { get; set; } // Vehicle VIN
        public string Name { get; set; } // "odometer", "battery_soc"
        public double Value { get; set; }
        public long Timestamp { get; set; }

        
    }
}
