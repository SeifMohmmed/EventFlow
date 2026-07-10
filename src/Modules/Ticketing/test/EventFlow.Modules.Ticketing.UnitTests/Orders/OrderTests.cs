using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Orders;

public class OrderTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenOrderIsCreated()
    {
        // Arrange - Create a customer.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Create an order.
        Result<Order> result = Order.Create(customer);

        // Assert - An order created domain event should be published.
        OrderCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderCreatedDomainEvent>(result.Value);

        domainEvent.OrderId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void IssueTicket_ShouldReturnFailure_WhenTicketAlreadyIssued()
    {
        // Arrange - Create an order and issue its tickets.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        result.Value.IssueTickets();
        Order order = result.Value;

        // Act - Attempt to issue tickets again.
        Result issueTicketsResult = order.IssueTickets();

        // Assert - The operation should fail because tickets were already issued.
        issueTicketsResult.Error.Should().Be(OrderErrors.TicketsAlreadyIssues);
    }

    [Fact]
    public void IssueTicket_ShouldRaiseDomainEvent_WhenTicketIsIssued()
    {
        // Arrange - Create an order.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        // Act - Issue tickets.
        result.Value.IssueTickets();

        // Assert - A tickets issued domain event should be published.
        OrderTicketsIssuedDomainEvent domainEvent =
            AssertDomainEventWasPublished<OrderTicketsIssuedDomainEvent>(result.Value);

        domainEvent.OrderId.Should().Be(result.Value.Id);
    }
}
