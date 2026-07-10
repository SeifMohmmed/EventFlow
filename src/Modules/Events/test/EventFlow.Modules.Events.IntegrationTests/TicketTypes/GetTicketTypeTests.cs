using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.TicketTypes;

// Integration tests for retrieving a single ticket type.
public class GetTicketTypeTests : BaseIntegrationTest
{
    public GetTicketTypeTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange - Use a ticket type that does not exist.
        var query = new GetTicketTypeQuery(Guid.NewGuid());

        // Act - Attempt to retrieve the ticket type.
        Result<TicketTypeResponse> result = await Sender.Send(query);

        // Assert - The ticket type should not be found.
        result.Error.Should().Be(TicketTypeErrors.NotFound(query.TicketTypeId));
    }

    [Fact]
    public async Task Should_ReturnTicketType_WhenTicketTypeExists()
    {
        // Arrange - Create a ticket type.
        await CleanDatabaseAsync();

        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());
        Guid eventId = await Sender.CreateEventAsync(categoryId);
        Guid ticketTypeId = await Sender.CreateTicketTypeAsync(eventId);

        var query = new GetTicketTypeQuery(ticketTypeId);

        // Act - Retrieve the ticket type.
        Result<TicketTypeResponse> result = await Sender.Send(query);

        // Assert - The ticket type should be returned successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
