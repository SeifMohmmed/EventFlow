using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Events.GetEvent;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.IntegrationEvents;
using MediatR;

namespace EventFlow.Modules.Events.Application.Events.PublishEvent;

internal sealed class EventPublishedDomainEventHandler(
    ISender sender,
    IEventBus eventBus)
    : DomainEventHandler<EventPublishedDomainEvent>
{
    public override async Task Handle(
        EventPublishedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the published event details.
        Result<EventResponse> result = await sender.Send(
            new GetEventQuery(domainEvent.EventId),
            cancellationToken);

        // Stop processing if the event cannot be found.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(GetEventQuery),
                result.Error);
        }

        // Publish an integration event so other modules
        // can react to the published event.
        await eventBus.PublishAsync(
            new EventPublishedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.Title,
                result.Value.Description,
                result.Value.Location,
                result.Value.StartsAtUtc,
                result.Value.EndsAtUtc,

                // Include all ticket types associated with the event.
                result.Value.TicketTypes
                    .Select(t => new TicketTypeModel
                    {
                        Id = t.TicketTypeId,
                        EventId = result.Value.Id,
                        Name = t.Name,
                        Price = t.Price,
                        Currency = t.Currency,
                        Quantity = t.Quantity
                    })
                    .ToList()),
            cancellationToken);
    }
}
