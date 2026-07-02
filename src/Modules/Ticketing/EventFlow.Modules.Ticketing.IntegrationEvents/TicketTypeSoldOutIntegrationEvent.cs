using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Ticketing.IntegrationEvents;

public sealed class TicketTypeSoldOutIntegrationEvent : IntegrationEvent
{
    public TicketTypeSoldOutIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid ticketTypeId)
        : base(id, occurredOnUtc)
    {
        TicketTypeId = ticketTypeId;
    }

    public Guid TicketTypeId { get; init; }
}
