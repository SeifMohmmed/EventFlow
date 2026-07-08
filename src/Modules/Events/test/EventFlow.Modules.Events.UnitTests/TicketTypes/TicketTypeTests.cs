using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.UnitTests.TicketTypes;

// Unit tests for the TicketType aggregate.
public class TicketTypeTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        // Arrange - Create an event that will own the ticket type.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> eventResult = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        // Act - Create a new ticket type.
        Result<TicketType> result = TicketType.Create(
            eventResult.Value,
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Random.String(),
            Faker.Random.Decimal());

        // Assert - Verify the ticket type was created successfully.
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void UpdatePrice_ShouldRaiseDomainEvent_WhenTicketTypeIsUpdated()
    {
        // Arrange - Create a ticket type.
        var category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> eventResult = Event.Create(
            category,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Result<TicketType> result = TicketType.Create(
            eventResult.Value,
            Faker.Music.Genre(),
            Faker.Random.Decimal(),
            Faker.Random.String(),
            Faker.Random.Decimal());

        TicketType ticketType = result.Value;

        // Act - Update the ticket price.
        ticketType.UpdatePrice(Faker.Random.Decimal());

        // Assert - Verify the TicketTypePriceChanged domain event was published.
        TicketTypePriceChangedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(ticketType);

        domainEvent.TicketTypeId.Should().Be(ticketType.Id);
    }
}
