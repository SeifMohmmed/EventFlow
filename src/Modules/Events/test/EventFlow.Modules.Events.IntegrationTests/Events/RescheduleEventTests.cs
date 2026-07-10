using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.PublishEvent;
using EventFlow.Modules.Events.Application.Events.RescheduleEvent;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for rescheduling events.
public class RescheduleEventTests : BaseIntegrationTest
{
    public RescheduleEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var eventId = Guid.NewGuid();

        var command = new PublishEventCommand(eventId);

        // Act - Attempt to reschedule the event.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(eventId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenStartDateIsInPast()
    {
        // Arrange - Create an event.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        DateTime startsAtUtc = DateTime.UtcNow.AddMinutes(-5);

        var command = new RescheduleEventCommand(
            eventId,
            startsAtUtc,
            null);

        // Act - Attempt to reschedule the event to the past.
        Result result = await Sender.Send(command);

        // Assert - Events cannot be rescheduled to a past date.
        result.Error.Should().Be(EventErrors.StartDateInPast);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsRescheduled()
    {
        // Arrange - Create a valid event.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var command = new RescheduleEventCommand(
            eventId,
            DateTime.UtcNow.AddMinutes(10),
            null);

        // Act - Reschedule the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be rescheduled successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
