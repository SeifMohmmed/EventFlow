using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Customers;

// Unit tests for the Customer aggregate.
public class CustomerTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenCustomerIsCreated()
    {
        // Arrange - Generate a unique customer identifier.
        var customerId = Guid.NewGuid();

        // Act - Create a new customer.
        Result<Customer> result = Customer.Create(
            customerId,
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Assert - The customer should be created successfully.
        result.Value.Should().NotBeNull();
    }
}
