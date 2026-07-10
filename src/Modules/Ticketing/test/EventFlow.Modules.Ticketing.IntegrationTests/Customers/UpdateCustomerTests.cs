using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.UpdateCustomer;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Customers;

// Integration tests for updating customers.
public class UpdateCustomerTests : BaseIntegrationTest
{
    public UpdateCustomerTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var command = new UpdateCustomerCommand(
            Guid.NewGuid(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Attempt to update the customer.
        Result result = await Sender.Send(command);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCustomerIsUpdated()
    {
        // Arrange - Create a customer.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new UpdateCustomerCommand(
            customerId,
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Update the customer.
        Result result = await Sender.Send(command);

        // Assert - The customer should be updated successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
