using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Domain.Events;
using EventFlow.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.UnitTests.Events;

// Unit tests for the Event aggregate.
public class EventTests : BaseTest
{
    [Fact]
    public void Should_RaiseDomainEvent_WhenEventCreated()
    {
        // Arrange - Prepare a valid event.
        var eventId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.Now;

        // Act - Create the event.
        Result<Event> result = Event.Create(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        // Assert - The EventCreated domain event should be published.
        EventCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCreatedDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }
}
