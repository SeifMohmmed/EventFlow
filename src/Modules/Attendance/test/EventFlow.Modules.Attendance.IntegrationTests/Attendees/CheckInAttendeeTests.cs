using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Attendees.CheckInAttendee;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Domain.Tickets;
using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.IntegrationTests.Attendees;

// Integration tests for attendee check-in.
public class CheckInAttendeeTests : BaseIntegrationTest
{
    public CheckInAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange - Use an attendee that does not exist.
        var command = new CheckInAttendeeCommand(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act - Attempt to check in the attendee.
        Result result = await Sender.Send(command);

        // Assert - The attendee should not be found.
        result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketDoesNotExist()
    {
        // Arrange - Create an attendee but use a non-existent ticket.
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());

        var command = new CheckInAttendeeCommand(
            attendeeId,
            Guid.NewGuid());

        // Act - Attempt to check in with an invalid ticket.
        Result result = await Sender.Send(command);

        // Assert - The ticket should not be found.
        result.Error.Should().Be(TicketErrors.NotFound);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeCheckedIn()
    {
        // Arrange - Create the attendee, event, and ticket.
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());
        Guid eventId = await Sender.CreateEventAsync(Guid.NewGuid());
        Guid ticketId = await Sender.CreateTicketAsync(
            Guid.NewGuid(),
            attendeeId,
            eventId);

        var command = new CheckInAttendeeCommand(
            attendeeId,
            ticketId);

        // Act - Check the attendee in.
        Result result = await Sender.Send(command);

        // Assert - The attendee should be checked in successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
