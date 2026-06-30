using EventFlow.Common.Domain;

namespace EventFlow.Modules.Ticketing.Domain.Tickets;

public sealed class TicketCreatedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;
}
