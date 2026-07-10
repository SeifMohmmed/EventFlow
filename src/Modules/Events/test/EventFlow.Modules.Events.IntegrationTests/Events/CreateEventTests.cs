using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.CreateEvent;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Events;

// Integration tests for event creation.
public class CreateEventTests : BaseIntegrationTest
{
    public CreateEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenStartDateInPast()
    {
        // Arrange - Build a command with a start date in the past.
        var command = new CreateEventCommand(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow.AddMinutes(-1),
            null);

        // Act - Execute the command.
        Result<Guid> result = await Sender.Send(command);

        // Assert - Events cannot start in the past.
        result.Error.Should().Be(EventErrors.StartDateInPast);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange - Use a category that does not exist.
        var categoryId = Guid.NewGuid();

        var command = new CreateEventCommand(
            categoryId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow.AddMinutes(10),
            null);

        // Act - Attempt to create the event.
        Result<Guid> result = await Sender.Send(command);

        // Assert - The category should not be found.
        result.Error.Should().Be(CategoryErrors.NotFound(categoryId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange - Build a command whose end date is before its start date.
        var categoryId = Guid.NewGuid();

        DateTime startsAtUtc = DateTime.UtcNow.AddMinutes(10);
        DateTime endsAtUtc = startsAtUtc.AddMinutes(-5);

        var command = new CreateEventCommand(
            categoryId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            endsAtUtc);

        // Act - Execute the command.
        Result<Guid> result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_CreateEvent_WhenCommandIsValid()
    {
        // Arrange - Create a category for the event.
        await CleanDatabaseAsync();

        Guid categoryId =
            await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new CreateEventCommand(
            categoryId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow.AddMinutes(10),
            null);

        // Act - Create the event.
        Result<Guid> result = await Sender.Send(command);

        // Assert - The event should be created successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
