using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.Application.Events.CreateEvent;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Events;

// Creates the corresponding Ticketing event after it has been published in the Events module.
internal sealed class EventPublishedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(
        EventPublishedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateEventCommand(
                integrationEvent.EventId,
                integrationEvent.Title,
                integrationEvent.Description,
                integrationEvent.Location,
                integrationEvent.StartsAtUtc,
                integrationEvent.EndsAtUtc,
                integrationEvent.TicketTypes
                    .Select(t => new CreateEventCommand.TicketTypeRequest(
                        t.Id,
                        integrationEvent.EventId,
                        t.Name,
                        t.Price,
                        t.Currency,
                        t.Quantity))
                    .ToList()),
            cancellationToken);

        // Throw to trigger the message bus retry/error handling pipeline.
        if (result.IsFailure)
        {
            throw new EventFlowException(nameof(CreateEventCommand), result.Error);
        }
    }
}
