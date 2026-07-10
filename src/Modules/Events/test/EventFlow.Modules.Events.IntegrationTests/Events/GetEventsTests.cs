using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.GetEvents;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for retrieving events.
public class GetEventsTests : BaseIntegrationTest
{
    public GetEventsTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventsDoNotExist()
    {
        // Arrange - Ensure the database contains no events.
        await CleanDatabaseAsync();

        var query = new GetEventsQuery();

        // Act - Retrieve all events.
        Result<IReadOnlyCollection<EventResponse>> result =
            await Sender.Send(query);

        // Assert - No events should be returned.
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnEvents_WhenEventsExist()
    {
        // Arrange - Create two events.
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        await Sender.CreateEventAsync(categoryId);
        await Sender.CreateEventAsync(categoryId);

        var query = new GetEventsQuery();

        // Act - Retrieve all events.
        Result<IReadOnlyCollection<EventResponse>> result =
            await Sender.Send(query);

        // Assert - Both events should be returned.
        result.Value.Should().HaveCount(2);
    }
}
