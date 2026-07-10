using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Events;

public class TicketTypeTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        // Arrange - Create an event that owns the ticket type.
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        // Act - Create a ticket type.
        Result<TicketType> result = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.String(3),
            Faker.Random.Decimal());

        // Assert - The ticket type should be created successfully.
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void UpdateQuantity_ShouldReturnFailure_WhenNotEnoughQuanitity()
    {
        // Arrange - Create an event and ticket type with a limited quantity.
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        decimal quantity = Faker.Random.Decimal();
        var ticketType = TicketType.Create(
        Guid.NewGuid(),
        @event.Id,
        Faker.Name.FirstName(),
        Faker.Random.Decimal(),
        Faker.Random.String(3),
        quantity);

        // Act - Attempt to reserve more tickets than are available.
        Result result = ticketType.UpdateQuantity(quantity + 1);

        // Assert - The operation should fail with a not enough quantity error.
        result.Error.Should().Be(TicketTypeErrors.NotEnoughQuantity(quantity));
    }

    [Fact]
    public void UpdateQuantity_ShouldRaiseDomainEvent_WhenTicketTypesIsSoldOut()
    {
        // Arrange - Create an event and ticket type.
        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        decimal quantity = Faker.Random.Decimal();
        Result<TicketType> ticketType = TicketType.Create(
        Guid.NewGuid(),
        @event.Id,
        Faker.Name.FirstName(),
        Faker.Random.Decimal(),
        Faker.Random.String(3),
        quantity);

        // Act - Sell the remaining quantity.
        ticketType.Value.UpdateQuantity(quantity);

        // Assert - A sold out domain event should be published.
        TicketTypeSoldOutDomainEvent domainEvent = AssertDomainEventWasPublished<TicketTypeSoldOutDomainEvent>(ticketType.Value);

        domainEvent.TicketTypeId.Should().Be(ticketType.Value.Id);
    }
}
