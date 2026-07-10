using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Tickets.CreateTicket;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Domain.Events;
using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.IntegrationTests.Tickets;

// Integration tests for ticket creation.
public class CreateTicketsTests : BaseIntegrationTest
{
    public CreateTicketsTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange - Use an attendee that does not exist.
        var command = new CreateTicketCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Faker.Random.String());

        // Act - Attempt to create the ticket.
        Result result = await Sender.Send(command);

        // Assert - The attendee should not be found.
        result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Create an attendee but use a non-existent event.
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());

        var command = new CreateTicketCommand(
            Guid.NewGuid(),
            attendeeId,
            Guid.NewGuid(),
            Faker.Random.String());

        // Act - Attempt to create the ticket.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(command.EventId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTicketIsCreated()
    {
        // Arrange - Create the attendee and event.
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());
        Guid eventId = await Sender.CreateEventAsync(Guid.NewGuid());

        var command = new CreateTicketCommand(
            Guid.NewGuid(),
            attendeeId,
            eventId,
            Ulid.NewUlid().ToString());

        // Act - Create the ticket.
        Result result = await Sender.Send(command);

        // Assert - The ticket should be created successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
