using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Events.IntegrationEvents;

// Published after all cancellation activities across modules have completed.
public sealed class EventCancellationCompletedIntegrationEvent : IntegrationEvent
{
    public EventCancellationCompletedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}
