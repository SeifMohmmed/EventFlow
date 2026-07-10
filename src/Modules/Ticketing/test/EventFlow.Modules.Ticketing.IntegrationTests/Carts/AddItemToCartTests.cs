using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Carts.AddItemToCart;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Carts;

// Integration tests for adding items to a customer's cart.
public class AddItemToCartTests : BaseIntegrationTest
{
    // Available quantity for the test ticket type.
    private const decimal Quantity = 10;

    public AddItemToCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var command = new AddItemToCartCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Random.Decimal());

        // Act - Attempt to add an item to the cart.
        Result result = await Sender.Send(command);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange - Create a customer but use a non-existing ticket type.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new AddItemToCartCommand(
            customerId,
            Guid.NewGuid(),
            Faker.Random.Decimal());

        // Act - Attempt to add the ticket type to the cart.
        Result result = await Sender.Send(command);

        // Assert - The ticket type should not be found.
        result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenNotEnoughQuantity()
    {
        // Arrange - Create a customer and an event with a limited ticket quantity.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new AddItemToCartCommand(
            customerId,
            ticketTypeId,
            Quantity + 1);

        // Act - Request more tickets than are available.
        Result result = await Sender.Send(command);

        // Assert - The request should fail due to insufficient quantity.
        result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(Quantity));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenItemAddedToCart()
    {
        // Arrange - Create a customer and an event with available tickets.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new AddItemToCartCommand(
            customerId,
            ticketTypeId,
            Quantity);

        // Act - Add the ticket to the customer's cart.
        Result result = await Sender.Send(command);

        // Assert - The item should be added successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
