using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Events;

// Unit tests for the Event aggregate.
public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenEventIsCreated()
    {
        // Arrange - Generate a unique event identifier.
        var eventId = Guid.NewGuid();

        // Act - Create a new event.
        Result<Event> result = Event.Create(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        // Assert - The event should be created successfully.
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventIsRescheduled()
    {
        // Arrange - Create a new event.
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        // Act - Reschedule the event.
        result.Value.Reschedule(
            startsAtUtc.AddDays(1),
            startsAtUtc.AddDays(2));

        // Assert - A rescheduled domain event should be published.
        EventRescheduledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventRescheduledDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventIsCanceled()
    {
        // Arrange - Create a new event.
        Result<Event> result = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        // Act - Cancel the event.
        result.Value.Cancel();

        // Assert - A canceled domain event should be published.
        EventCanceledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCanceledDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsAreRefunded()
    {
        // Arrange - Create a new event.
        Result<Event> result = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        // Act - Mark the event's payments as refunded.
        result.Value.PaymentsRefunded();

        // Assert - A payments refunded domain event should be published.
        EventPaymentsRefundedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventPaymentsRefundedDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void TicketsArchived_ShouldRaiseDomainEvent_WhenTicketsAreArchived()
    {
        // Arrange - Create a new event.
        Result<Event> result = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow,
            null);

        // Act - Mark the event's tickets as archived.
        result.Value.TicketsArchived();

        // Assert - A tickets archived domain event should be published.
        EventTicketsArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventTicketsArchivedDomainEvent>(result.Value);

        domainEvent.EventId.Should().Be(result.Value.Id);
    }
}
