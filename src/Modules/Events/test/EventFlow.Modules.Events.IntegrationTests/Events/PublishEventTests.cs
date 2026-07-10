using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.PublishEvent;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for publishing events.
public class PublishEventTests : BaseIntegrationTest
{
    public PublishEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var eventId = Guid.NewGuid();

        var command = new PublishEventCommand(eventId);

        // Act - Attempt to publish the event.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(eventId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotHaveAnyTicketTypes()
    {
        // Arrange - Create an event without ticket types.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var command = new PublishEventCommand(eventId);

        // Act - Attempt to publish the event.
        Result result = await Sender.Send(command);

        // Assert - Events without ticket types cannot be published.
        result.Error.Should().Be(EventErrors.NoTicketsFound);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsPublished()
    {
        // Arrange - Create an event with at least one ticket type.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        await Sender.CreateTicketTypeAsync(eventId);

        var command = new PublishEventCommand(eventId);

        // Act - Publish the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be published successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
