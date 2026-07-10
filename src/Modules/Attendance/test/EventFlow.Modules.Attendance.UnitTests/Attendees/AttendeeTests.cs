using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Domain.Events;
using EventFlow.Modules.Attendance.Domain.Tickets;
using EventFlow.Modules.Attendance.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.UnitTests.Attendees;

// Unit tests for the Attendee aggregate.
public class AttendeeTests : BaseTest
{
    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketIsNotValid()
    {
        // Arrange - Create two different attendees.
        var attendee = Attendee.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        var ticketAttendee = Attendee.Create(
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

        // Create a ticket that belongs to another attendee.
        var ticket = Ticket.Create(
            Guid.NewGuid(),
            ticketAttendee,
            @event,
            Faker.Random.String());

        // Act - Attempt to check in using another attendee's ticket.
        Result checkInAttendee = attendee.CheckIn(ticket);

        // Assert - An invalid check-in event should be published.
        InvalidCheckInAttemptedDomainEvent domainEvent =
            AssertDomainEventWasPublished<InvalidCheckInAttemptedDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);

        checkInAttendee.Error.Should().Be(TicketErrors.InvalidCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketAlreadyUsed()
    {
        // Arrange - Create an attendee and a valid ticket.
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

        // Mark the ticket as already used.
        ticket.MarkAsUsed();

        // Act - Attempt to check in again.
        Result checkInAttendee = attendee.CheckIn(ticket);

        // Assert - A duplicate check-in event should be published.
        DuplicateCheckInAttemptedDomainEvent domainEvent =
            AssertDomainEventWasPublished<DuplicateCheckInAttemptedDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);

        checkInAttendee.Error.Should().Be(TicketErrors.DuplicateCheckIn);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenSuccessfullyCheckedIn()
    {
        // Arrange - Create an attendee and a valid ticket.
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

        // Act - Check the attendee in.
        attendee.CheckIn(ticket);

        // Assert - The AttendeeCheckedIn domain event should be published.
        AttendeeCheckedInDomainEvent domainEvent =
            AssertDomainEventWasPublished<AttendeeCheckedInDomainEvent>(attendee);

        domainEvent.AttendeeId.Should().Be(attendee.Id);
    }
}
