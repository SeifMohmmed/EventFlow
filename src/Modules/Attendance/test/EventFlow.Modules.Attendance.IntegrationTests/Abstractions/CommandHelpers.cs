using Bogus;
using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Attendees.CreateAttendee;
using EventFlow.Modules.Attendance.Application.Events.CreateEvent;
using EventFlow.Modules.Attendance.Application.Tickets.CreateTicket;
using FluentAssertions;
using MediatR;

namespace EventFlow.Modules.Attendance.IntegrationTests.Abstractions;

// Helper methods for creating test data.
internal static class CommandHelpers
{
    // Creates an attendee for integration tests.
    internal static async Task<Guid> CreateAttendeeAsync(
        this ISender sender,
        Guid attendeeId)
    {
        var faker = new Faker();

        Result result = await sender.Send(
            new CreateAttendeeCommand(
                attendeeId,
                faker.Internet.Email(),
                faker.Name.FirstName(),
                faker.Name.LastName()));

        // Ensure setup completed successfully.
        result.IsSuccess.Should().BeTrue();

        return attendeeId;
    }

    // Creates a ticket for integration tests.
    internal static async Task<Guid> CreateTicketAsync(
        this ISender sender,
        Guid ticketId,
        Guid attendeeId,
        Guid eventId)
    {
        Result result = await sender.Send(
            new CreateTicketCommand(
                ticketId,
                attendeeId,
                eventId,
                Ulid.NewUlid().ToString()));

        // Ensure setup completed successfully.
        result.IsSuccess.Should().BeTrue();

        return ticketId;
    }

    // Creates an event for integration tests.
    internal static async Task<Guid> CreateEventAsync(
        this ISender sender,
        Guid eventId)
    {
        var faker = new Faker();

        Result result = await sender.Send(
            new CreateEventCommand(
                eventId,
                faker.Music.Genre(),
                faker.Music.Genre(),
                faker.Address.StreetAddress(),
                DateTime.UtcNow.AddMinutes(10),
                null));

        // Ensure setup completed successfully.
        result.IsSuccess.Should().BeTrue();

        return eventId;
    }
}
