using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.UnitTests.Events;

// Unit tests for the Event aggregate.
public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange - Create an invalid date range.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.Now;
        DateTime endsAtUtc = startsAtUtc.AddDays(-1);

        // Act - Attempt to create the event.
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            endsAtUtc);

        // Assert - The creation should fail because the end date is before the start date.
        result.Error.Should().Be(EventErrors.EndDatePrecedesStartDate);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenEventCreated()
    {
        // Arrange - Create a valid event request.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.Now;

        // Act - Create the event.
        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        // Assert - Verify the EventCreated domain event was published.
        EventCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCreatedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNotDraft()
    {
        // Arrange - Create and publish the event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Publish();

        // Act - Attempt to publish the event again.
        Result publishResult = @event.Publish();

        // Assert - Publishing a non-draft event should fail.
        publishResult.Error.Should().Be(EventErrors.NotDraft);
    }

    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        // Arrange - Create a draft event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        // Act - Publish the event.
        @event.Publish();

        // Assert - Verify the EventPublished domain event was published.
        EventPublishedDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventPublishedDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        // Arrange - Create an event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        // Act - Reschedule the event.
        @event.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        // Assert - Verify the EventRescheduled domain event was published.
        EventRescheduledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        // Arrange - Create an event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        // Act - Cancel the event before it starts.
        @event.Cancel(startsAtUtc.AddMinutes(-1));

        // Assert - Verify the EventCanceled domain event was published.
        EventCanceledDomainEvent domainEvent =
            AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event);

        domainEvent.EventId.Should().Be(@event.Id);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
    {
        // Arrange - Create and cancel the event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        @event.Cancel(startsAtUtc.AddMinutes(-1));

        // Act - Attempt to cancel the event again.
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(-1));

        // Assert - An already canceled event cannot be canceled again.
        cancelResult.Error.Should().Be(EventErrors.AlreadyCanceled);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange - Create an event.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event @event = result.Value;

        // Act - Attempt to cancel the event after its start time.
        Result cancelResult = @event.Cancel(startsAtUtc.AddMinutes(1));

        // Assert - Started events cannot be canceled.
        cancelResult.Error.Should().Be(EventErrors.AlreadyStarted);
    }
}
