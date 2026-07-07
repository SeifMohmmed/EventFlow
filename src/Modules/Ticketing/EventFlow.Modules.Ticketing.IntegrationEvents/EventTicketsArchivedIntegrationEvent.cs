using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Ticketing.IntegrationEvents;

// Indicates that all tickets belonging to an event have been archived.
public sealed class EventTicketsArchivedIntegrationEvent : IntegrationEvent
{
    public EventTicketsArchivedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid eventId)
        : base(id, occurredOnUtc)
    {
        EventId = eventId;
    }

    public Guid EventId { get; init; }
}
