using EventFlow.Common.Domain;

namespace EventFlow.Common.Application.Messaging;

/// <summary>
/// Defines a handler for domain events.
/// </summary>
/// <typeparam name="TDomainEvent">
/// The type of domain event to handle.
/// </typeparam>
public interface IDomainEventHandler<in TDomainEvent> : IDomainEventHandler
    where TDomainEvent : IDomainEvent
{
    // Handles the specified domain event.
    Task Handle(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default);
}

public interface IDomainEventHandler
{
    // Handles a domain event without requiring its concrete type.
    Task Handle(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default);
}
