using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
using MassTransit;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.TicketTypes;

public sealed class TicketTypePriceChangedIntegrationEventConsumer(ISender sender)
    : IConsumer<TicketTypePriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<TicketTypePriceChangedIntegrationEvent> context)
    {
        // Update the ticket type price in the Ticketing module.
        Result result = await sender.Send(
            new UpdateTicketTypePriceCommand(
                context.Message.TicketTypeId,
                context.Message.Price),
            context.CancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(UpdateTicketTypePriceCommand),
                result.Error);
        }
    }
}
