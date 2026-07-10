using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.TicketTypes;

// Integration tests for updating ticket type prices.
public class UpdateTicketTypeTests : BaseIntegrationTest
{
    public UpdateTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange - Use a ticket type that does not exist.
        var command = new UpdateTicketTypePriceCommand(
            Guid.NewGuid(),
            Faker.Random.Decimal());

        // Act - Attempt to update the ticket type price.
        Result result = await Sender.Send(command);

        // Assert - The ticket type should not be found.
        result.Error.Should().Be(TicketTypeErrors.NotFound(command.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTicketTypeExists()
    {
        // Arrange - Create a ticket type.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);
        Guid ticketTypeId = await Sender.CreateTicketTypeAsync(eventId);

        var command = new UpdateTicketTypePriceCommand(
            ticketTypeId,
            Faker.Random.Decimal());

        // Act - Update the ticket type price.
        Result result = await Sender.Send(command);

        // Assert - The ticket type should be updated successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
