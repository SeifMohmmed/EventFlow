using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.TicketTypes;

internal sealed class TicketTypePriceChangedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<TicketTypePriceChangedIntegrationEvent>
{
    public override async Task Handle(
        TicketTypePriceChangedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateTicketTypePriceCommand(integrationEvent.TicketTypeId, integrationEvent.Price),
            cancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(UpdateTicketTypePriceCommand),
                result.Error);
        }
    }
}
