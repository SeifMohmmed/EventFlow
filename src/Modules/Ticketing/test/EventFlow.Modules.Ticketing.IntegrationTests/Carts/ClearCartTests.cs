using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Carts.ClearCart;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Carts;

// Integration tests for clearing a customer's cart.
public class ClearCartTests : BaseIntegrationTest
{
    public ClearCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var command = new ClearCartCommand(Guid.NewGuid());

        // Act - Attempt to clear the cart.
        Result result = await Sender.Send(command);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCustomerExists()
    {
        // Arrange - Create a customer.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new ClearCartCommand(customerId);

        // Act - Clear the customer's cart.
        Result result = await Sender.Send(command);

        // Assert - The cart should be cleared successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
