using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.GetEvent;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for retrieving a single event.
public class GetEventTests : BaseIntegrationTest
{
    public GetEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var query = new GetEventQuery(Guid.NewGuid());

        // Act - Attempt to retrieve the event.
        Result<EventResponse> result = await Sender.Send(query);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(query.EventId));
    }

    [Fact]
    public async Task Should_ReturnEvent_WhenEventExists()
    {
        // Arrange - Create an event.
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var query = new GetEventQuery(eventId);

        // Act - Retrieve the event.
        Result<EventResponse> result = await Sender.Send(query);

        // Assert - The event should be returned successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
