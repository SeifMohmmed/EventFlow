using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Ticketing.IntegrationEvents;

/// <summary>
/// Integration event published when a ticket is archived.
/// </summary>
public sealed class TicketArchivedIntegrationEvent : IntegrationEvent
{
    public TicketArchivedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid ticketId,
        string code)
        : base(id, occurredOnUtc)
    {
        TicketId = ticketId;
        Code = code;
    }

    // The archived ticket identifier.
    public Guid TicketId { get; init; }

    // The archived ticket code.
    public string Code { get; init; }
}
