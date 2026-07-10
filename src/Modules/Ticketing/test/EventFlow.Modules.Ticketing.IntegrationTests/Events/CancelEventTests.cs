using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Events.CancelEvent;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Events;

// Integration tests for event cancellation.
public class CancelEventTests : BaseIntegrationTest
{
    // Available quantity for the test ticket type.
    private const decimal Quantity = 10;

    public CancelEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var eventId = Guid.NewGuid();

        var command = new CancelEventCommand(eventId);

        // Act - Attempt to cancel the event.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(command.EventId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCanceled()
    {
        // Arrange - Create an event with a ticket type.
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new CancelEventCommand(eventId);

        // Act - Cancel the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be canceled successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
