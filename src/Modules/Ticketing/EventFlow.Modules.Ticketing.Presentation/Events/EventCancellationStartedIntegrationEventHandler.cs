using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.Events.CancelEvent;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Events;

// Starts the ticketing module's cancellation process when notified by the Events module.
internal sealed class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(
        EventCancellationStartedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result =
            await sender.Send(new CancelEventCommand(integrationEvent.EventId), cancellationToken);

        // Surface failures so the message bus can apply its retry policy.
        if (result.IsFailure)
        {
            throw new EventFlowException(nameof(CancelEventCommand), result.Error);
        }
    }
}
