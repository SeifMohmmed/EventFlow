using Bogus;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Events.CreateEvent;
using FluentAssertions;
using MediatR;

namespace EventFlow.IntegrationTests.Abstractions;

// Helper methods used by integration tests.
internal static class CommandHelpers
{
    // Creates an event with a single ticket type for testing purposes.
    internal static async Task CreateEventAsync(
        this ISender sender,
        Guid eventId,
        Guid ticketTypeId,
        decimal quantity)
    {
        var faker = new Faker();

        var ticketType = new CreateEventCommand.TicketTypeRequest(
            ticketTypeId,
            eventId,
            faker.Music.Genre(),
            faker.Random.Decimal(),
            "USD",
            quantity);

        Result result = await sender.Send(
            new CreateEventCommand(
                eventId,
                faker.Music.Genre(),
                faker.Music.Genre(),
                faker.Address.FullAddress(),
                DateTime.UtcNow,
                null,
                [ticketType]));

        // Ensure the event is created successfully before continuing the test.
        result.IsSuccess.Should().BeTrue();
    }
}
