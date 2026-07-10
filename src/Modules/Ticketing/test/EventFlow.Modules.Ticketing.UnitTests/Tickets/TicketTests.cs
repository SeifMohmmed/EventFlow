using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.Domain.Tickets;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Tickets;

// Unit tests for the Ticket aggregate.
public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange - Create the required customer, order, event, and ticket type.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.String(3),
            Faker.Random.Decimal());

        // Act - Create a ticket.
        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        // Assert - Verify the ticket created domain event was published.
        TicketCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenTicketIsArchived()
    {
        // Arrange - Create a ticket.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;
        var @event = Event.Create(
            Guid.NewGuid(),
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        var ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.String(3),
            Faker.Random.Decimal());

        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        // Act - Archive the ticket.
        result.Value.Archive();

        // Assert - Verify the ticket archived domain event was published.
        TicketArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<TicketArchivedDomainEvent>(result.Value);

        domainEvent.TicketId.Should().Be(result.Value.Id);
    }
}
