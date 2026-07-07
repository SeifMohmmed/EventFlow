using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Ticketing.IntegrationEvents;

// Indicates that all refunds for an event have been processed.
public sealed class EventPaymentsRefundedIntegrationEvent : IntegrationEvent
{
    public EventPaymentsRefundedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}
