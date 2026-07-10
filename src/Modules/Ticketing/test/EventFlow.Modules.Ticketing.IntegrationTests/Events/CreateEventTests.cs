using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Events.CreateEvent;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Events;

// Integration tests for event creation.
public class CreateEventTests : BaseIntegrationTest
{
    public CreateEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCreated()
    {
        // Arrange - Build a valid event creation command.
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();
        decimal quantity = Faker.Random.Decimal();

        var ticketType = new CreateEventCommand.TicketTypeRequest(
            ticketTypeId,
            eventId,
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Random.String(3),
            quantity);

        var command = new CreateEventCommand(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.FullAddress(),
            DateTime.UtcNow,
            null,
            [ticketType]);

        // Act - Create the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be created successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
