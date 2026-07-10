using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Carts;
using EventFlow.Modules.Ticketing.Application.Orders.CreateOrder;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Orders;

// Integration tests for order creation.
public class CreateOrderTests : BaseIntegrationTest
{
    public CreateOrderTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var command = new CreateOrderCommand(Guid.NewGuid());

        // Act - Attempt to create an order.
        Result result = await Sender.Send(command);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCartIsEmpty()
    {
        // Arrange - Create a customer with an empty shopping cart.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new CreateOrderCommand(customerId);

        // Act - Attempt to create an order.
        Result result = await Sender.Send(command);

        // Assert - Orders cannot be created from an empty cart.
        result.Error.Should().Be(CartErrors.Empty);
    }
}
