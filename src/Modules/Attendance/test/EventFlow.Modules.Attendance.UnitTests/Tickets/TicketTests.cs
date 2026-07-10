using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Domain.Events;
using EventFlow.Modules.Attendance.Domain.Tickets;
using EventFlow.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.UnitTests.Tickets;

// Unit tests for the Ticket aggregate.
public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange - Create the attendee and event.
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetName(),
            startsAtUtc,
            null);

        // Act - Create the ticket.
        Result<Ticket> result = Ticket.Create(
            Guid.NewGuid(),
            attendee,
            @event,
            Faker.Random.String());

        // Assert - The TicketCreated domain event should be published.
        TicketCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void MarkAsUsed_ShouldRaiseDomainEvent_WhenTicketIsUsed()
    {
        // Arrange - Create a valid ticket.
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetName(),
            startsAtUtc,
            null);

        var ticket = Ticket.Create(
            Guid.NewGuid(),
            attendee,
            @event,
            Faker.Random.String());

        // Act - Mark the ticket as used.
        ticket.MarkAsUsed();

        // Assert - The TicketUsed domain event should be published.
        TicketUsedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketUsedDomainEvent>(ticket);

        domainEvent.TicketId.Should().Be(ticket.Id);
    }
}
