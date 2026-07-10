using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketTypes;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.TicketTypes;

// Integration tests for retrieving ticket types.
public class GetTicketTypesTests : BaseIntegrationTest
{
    public GetTicketTypesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypesDoNotExist()
    {
        // Arrange - Ensure the database contains no ticket types.
        await CleanDatabaseAsync();

        var query = new GetTicketTypesQuery(Guid.NewGuid());

        // Act - Retrieve ticket types for the event.
        Result<IReadOnlyCollection<TicketTypeResponse>> result =
            await Sender.Send(query);

        // Assert - No ticket types should be returned.
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnTicketTypes_WhenTicketTypesExists()
    {
        // Arrange - Create an event with two ticket types.
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        await Sender.CreateTicketTypeAsync(eventId);
        await Sender.CreateTicketTypeAsync(eventId);

        var query = new GetTicketTypesQuery(eventId);

        // Act - Retrieve the ticket types.
        Result<IReadOnlyCollection<TicketTypeResponse>> result =
            await Sender.Send(query);

        // Assert - Both ticket types should be returned.
        result.Value.Should().HaveCount(2);
    }
}
