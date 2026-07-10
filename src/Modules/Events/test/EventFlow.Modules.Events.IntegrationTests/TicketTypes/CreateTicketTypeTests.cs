using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.CreateTicketType;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.TicketTypes;

// Integration tests for ticket type creation.
public class CreateTicketTypeTests : BaseIntegrationTest
{
    public CreateTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange - Use an event that does not exist.
        var command = new CreateTicketTypeCommand(
            Guid.NewGuid(),
            Faker.Commerce.ProductName(),
            Faker.Random.Decimal(),
            "USD",
            Faker.Random.Decimal());

        // Act - Attempt to create the ticket type.
        Result<Guid> result = await Sender.Send(command);

        // Assert - The event should not be found.
        result.Error.Should().Be(EventErrors.NotFound(command.EventId));
    }

    [Fact]
    public async Task Should_CreateTicketType_WhenCommandIsValid()
    {
        // Arrange - Create an event.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);

        var command = new CreateTicketTypeCommand(
            eventId,
            Faker.Commerce.ProductName(),
            Faker.Random.Decimal(),
            "USD",
            Faker.Random.Decimal());

        // Act - Create the ticket type.
        Result<Guid> result = await Sender.Send(command);

        // Assert - The ticket type should be created successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}
