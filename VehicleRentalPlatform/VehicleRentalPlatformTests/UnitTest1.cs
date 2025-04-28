using Microsoft.AspNetCore.Mvc;
using Moq;
using VehicleRentalPlatform.Domain;
using VehicleRentalPlatform.Infrastructure.Persistence;
using VehicleRentalPlatform.API.Controllers; 
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Tests
{
    public class CustomersControllerTests
    {
        private readonly CustomersController _controller;
        private readonly AppDbContext _context;

        public CustomersControllerTests()
        {
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options);
            _controller = new CustomersController(_context);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedResult()
        {
            // Arrange
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Test Test"
            };

            // Act
            var result = await _controller.Create(customer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(customer.Name, returnValue.Name);
        }
    }
}
