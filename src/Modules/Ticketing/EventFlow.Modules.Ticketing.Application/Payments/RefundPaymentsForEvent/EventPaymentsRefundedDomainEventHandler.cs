using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.IntegrationEvents;

namespace EventFlow.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

// Publishes an integration event after all event payments have been refunded.
internal sealed class EventPaymentsRefundedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventPaymentsRefundedDomainEvent>
{
    public override async Task Handle(
        EventPaymentsRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await eventBus.PublishAsync(
            new EventPaymentsRefundedIntegrationEvent(
                domainEvent.EventId,
                domainEvent.OccurredOnUtc,
                domainEvent.EventId),
            cancellationToken);
    }
}
