using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Events.RescheduleEvent;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Events;

// Integration tests for event rescheduling.
public class RescheduleEventTests : BaseIntegrationTest
{
    // Available quantity for the test ticket type.
    private const decimal Quantity = 10;

    public RescheduleEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var eventId = Guid.NewGuid();

        DateTime startsAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        var command = new RescheduleEventCommand(
            eventId,
            startsAtUtc,
            endsAtUtc);

        // Act - Attempt to reschedule the event.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(command.EventId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange - Create an event and reschedule it to a past date.
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        DateTime startsAtUtc = DateTime.UtcNow.AddMinutes(-5);
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new RescheduleEventCommand(
            eventId,
            startsAtUtc,
            endsAtUtc);

        // Act - Attempt to reschedule the event.
        Result result = await Sender.Send(command);

        // Assert - Past start dates are not allowed.
        result.Error.Should().Be(EventErrors.StartDateInPast);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventRescheduled()
    {
        // Arrange - Create an event and reschedule it to a valid future date.
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        DateTime startsAtUtc = DateTime.UtcNow.AddHours(1);
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        await Sender.CreateEventWithTicketTypeAsync(
            eventId,
            ticketTypeId,
            Quantity);

        var command = new RescheduleEventCommand(
            eventId,
            startsAtUtc,
            endsAtUtc);

        // Act - Reschedule the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be rescheduled successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
