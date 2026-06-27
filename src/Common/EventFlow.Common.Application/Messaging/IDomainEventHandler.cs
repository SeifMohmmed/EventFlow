using EventFlow.Common.Domain;
using MediatR;

namespace EventFlow.Common.Application.Messaging;

/// <summary>
/// Defines a handler for domain events.
/// </summary>
/// <typeparam name="TDomainEvent">
/// The type of domain event to handle.
/// </typeparam>
public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
