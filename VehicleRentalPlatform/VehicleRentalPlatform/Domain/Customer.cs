namespace VehicleRentalPlatform.Domain
{
    public class Customer
    {
        public Guid Id { get; set; } // Primary Key
        public string Name { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
