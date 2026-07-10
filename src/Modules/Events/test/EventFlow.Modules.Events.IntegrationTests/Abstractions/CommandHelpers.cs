using Bogus;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.CreateCategory;
using EventFlow.Modules.Events.Application.Events.CreateEvent;
using EventFlow.Modules.Events.Application.TicketTypes.CreateTicketType;
using MediatR;

namespace EventFlow.Modules.Events.IntegrationTests.Abstractions;

// Helper methods for creating test data.
internal static class CommandHelpers
{
    // Creates a category for integration tests.
    internal static async Task<Guid> CreateCategoryAsync(
        this ISender sender,
        string name)
    {
        Result<Guid> result =
            await sender.Send(new CreateCategoryCommand(name));

        return result.Value;
    }

    // Creates an event for integration tests.
    internal static async Task<Guid> CreateEventAsync(
        this ISender sender,
        Guid categoryId,
        DateTime? startsAtUtc = null)
    {
        var faker = new Faker();

        Result<Guid> result = await sender.Send(
            new CreateEventCommand(
                categoryId,
                faker.Music.Genre(),
                faker.Music.Genre(),
                faker.Address.StreetAddress(),
                startsAtUtc ?? DateTime.UtcNow.AddMinutes(10),
                null));

        return result.Value;
    }

    // Creates a ticket type for integration tests.
    internal static async Task<Guid> CreateTicketTypeAsync(
        this ISender sender,
        Guid eventId)
    {
        var faker = new Faker();

        Result<Guid> result = await sender.Send(
            new CreateTicketTypeCommand(
                eventId,
                faker.Commerce.ProductName(),
                faker.Random.Decimal(),
                "USD",
                faker.Random.Decimal()));

        return result.Value;
    }
}
