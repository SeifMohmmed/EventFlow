using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationEvents;

namespace EventFlow.Modules.Events.Application.Events.CancelEvent;

// Publishes an integration event after an event has been canceled.
// This allows other modules to participate in the cancellation workflow.
internal sealed class EventCanceledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventCanceledIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
