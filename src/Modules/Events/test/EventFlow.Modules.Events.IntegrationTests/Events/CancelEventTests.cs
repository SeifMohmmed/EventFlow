using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.CancelEvent;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;
using NSubstitute;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for event cancellation.
public class CancelEventTests : BaseIntegrationTest
{
    public CancelEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var eventId = Guid.NewGuid();

        var command = new CancelEventCommand(eventId);

        // Act - Attempt to cancel the event.
        Result result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(eventId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyCanceled()
    {
        // Arrange - Create and cancel an event.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var command = new CancelEventCommand(eventId);

        await Sender.Send(command);

        // Act - Attempt to cancel the event again.
        Result result = await Sender.Send(command);

        // Assert - The event is already canceled.
        result.Error.Should().Be(EventErrors.AlreadyCanceled);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange - Create an event that starts in five minutes.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        Guid eventId = await Sender.CreateEventAsync(
            categoryId,
            DateTime.UtcNow.AddMinutes(5));

        // Simulate time after the event has already started.
        Factory.DateTimeProviderMock.UtcNow
            .Returns(DateTime.UtcNow.AddMinutes(15));

        var command = new CancelEventCommand(eventId);

        // Act - Attempt to cancel the event.
        Result result = await Sender.Send(command);

        // Assert - Started events cannot be canceled.
        result.Error.Should().Be(EventErrors.AlreadyStarted);

        // Restore the default clock for subsequent tests.
        Factory.DateTimeProviderMock.UtcNow
            .Returns(_ => DateTime.UtcNow);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCanceled()
    {
        // Arrange - Create a valid event.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var command = new CancelEventCommand(eventId);

        // Act - Cancel the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be canceled successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
