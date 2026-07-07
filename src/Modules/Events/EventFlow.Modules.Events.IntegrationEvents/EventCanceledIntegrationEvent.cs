using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Events.IntegrationEvents;

// Raised when an event has been canceled.
public sealed class EventCanceledIntegrationEvent : IntegrationEvent
{
    public EventCanceledIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    // Identifier of the canceled event.
    public Guid EventId { get; init; }
}
