using Bogus;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.CreateCustomer;
using EventFlow.Modules.Ticketing.Application.Events.CreateEvent;
using FluentAssertions;
using MediatR;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;

// Helper methods for creating test data.
internal static class CommandHelpers
{
    // Creates a customer for integration tests.
    internal static async Task<Guid> CreateCustomerAsync(
        this ISender sender,
        Guid customerId)
    {
        var faker = new Faker();

        Result result = await sender.Send(
            new CreateCustomerCommand(
                customerId,
                faker.Internet.Email(),
                faker.Person.FirstName,
                faker.Person.LastName));

        // Ensure setup completed successfully.
        result.IsSuccess.Should().BeTrue();

        return customerId;
    }

    // Creates an event with a single ticket type for integration tests.
    internal static async Task CreateEventWithTicketTypeAsync(
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

        // Ensure setup completed successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
