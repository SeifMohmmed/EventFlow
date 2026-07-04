using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.Events.CreateEvent;
using MassTransit;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Events;

public sealed class EventPublishedIntegrationEventConsumer(ISender sender)
    : IConsumer<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        // Create the published event in the Ticketing module.
        Result result = await sender.Send(
            new CreateEventCommand(
                context.Message.EventId,
                context.Message.Title,
                context.Message.Description,
                context.Message.Location,
                context.Message.StartsAtUtc,
                context.Message.EndsAtUtc,

                // Map the published ticket types.
                context.Message.TicketTypes
                    .Select(t => new CreateEventCommand.TicketTypeRequest(
                        t.Id,
                        context.Message.EventId,
                        t.Name,
                        t.Price,
                        t.Currency,
                        t.Quantity))
                    .ToList()),
            context.CancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(CreateEventCommand),
                result.Error);
        }
    }
}
