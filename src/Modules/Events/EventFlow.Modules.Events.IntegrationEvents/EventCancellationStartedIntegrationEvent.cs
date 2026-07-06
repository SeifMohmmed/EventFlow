using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Events.IntegrationEvents;

// Integration event published when the event cancellation process begins.
// Other modules consume this event to perform their own cancellation logic.
public sealed class EventCancellationStartedIntegrationEvent : IntegrationEvent
{
    public EventCancellationStartedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    // The event being cancelled.
    public Guid EventId { get; init; }
}
