using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationEvents;

namespace EventFlow.Modules.Events.Application.Events.RescheduleEvent;

/// <summary>
/// Handles the <see cref="EventRescheduledDomainEvent"/>.
/// </summary>
internal sealed class EventRescheduledDomainEventHandler(
    IEventBus eventBus) : DomainEventHandler<EventRescheduledDomainEvent>
{
    /// <summary>
    /// Executes the logic that should run after an event
    /// has been rescheduled.
    /// </summary>
    public override async Task Handle(
        EventRescheduledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventRescheduledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc),
            cancellationToken);
    }
}
