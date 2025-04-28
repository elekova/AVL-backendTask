using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Domain;
using VehicleRentalPlatform.Infrastructure.Persistence;
using VehicleRentalPlatform.API.Controllers; // prilagodi put ako treba
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace VehicleRentalPlatform.Tests
{
    public class CustomersIntegrationTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // svjež DB za svaki test
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateCustomer_ShouldCreateCustomerSuccessfully()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new CustomersController(context);

            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Test Test"
            };

            // Act
            var result = await controller.Create(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCustomer = Assert.IsType<Customer>(createdAtActionResult.Value);

            Assert.Equal(newCustomer.Name, createdCustomer.Name);

            // Provjerimo direktno u bazi
            var customerInDb = await context.Customers.FindAsync(createdCustomer.Id);
            Assert.NotNull(customerInDb);
            Assert.Equal(newCustomer.Name, customerInDb.Name);
        }
    }
}