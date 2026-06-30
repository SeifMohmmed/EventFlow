using EventFlow.Common.Domain;

namespace EventFlow.Modules.Ticketing.Domain.Orders;

public sealed class OrderTicketsIssuedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
