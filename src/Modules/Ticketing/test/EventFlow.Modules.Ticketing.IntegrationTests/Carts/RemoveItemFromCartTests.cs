using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Carts.RemoveItemFromCart;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Carts;

// Integration tests for removing items from a customer's cart.
public class RemoveItemFromCartTests : BaseIntegrationTest
{
    // Available quantity for the test ticket type.
    private const decimal Quantity = 10;

    public RemoveItemFromCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var command = new RemoveItemFromCartCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act - Attempt to remove an item from the cart.
        Result result = await Sender.Send(command);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange - Create a customer but use a non-existing ticket type.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new RemoveItemFromCartCommand(
            customerId,
            Guid.NewGuid());

        // Act - Attempt to remove the ticket type.
        Result result = await Sender.Send(command);

        // Assert - The ticket type should not be found.
        result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenRemovedItemFromCart()
    {
        // Arrange - Create a customer and an event with an available ticket type.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new RemoveItemFromCartCommand(
            customerId,
            ticketTypeId);

        // Act - Remove the ticket type from the cart.
        Result result = await Sender.Send(command);

        // Assert - The item should be removed successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
