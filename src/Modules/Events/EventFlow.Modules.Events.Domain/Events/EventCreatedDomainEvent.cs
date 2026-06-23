using EventFlow.Modules.Events.Domain.Abstractions;

namespace EventFlow.Modules.Events.Domain;

public sealed class EventCreatedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
