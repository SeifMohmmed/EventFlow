using EventFlow.Common.Domain;

namespace EventFlow.Common.Application.Messaging;

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    // Handles the strongly typed domain event.
    public abstract Task Handle(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default);

    public Task Handle(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Cast the event to its concrete type and delegate to the typed handler.
        return Handle((TDomainEvent)domainEvent, cancellationToken);
    }
}
