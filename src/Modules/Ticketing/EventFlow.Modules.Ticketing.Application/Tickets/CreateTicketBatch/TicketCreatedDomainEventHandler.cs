using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Tickets.GetTicket;
using EventFlow.Modules.Ticketing.Domain.Tickets;
using EventFlow.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class TicketCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the newly created ticket.
        Result<TicketResponse> result = await sender.Send(
            new GetTicketQuery(domainEvent.TicketId),
            cancellationToken);

        // Stop processing if the ticket cannot be found.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(GetTicketQuery),
                result.Error);
        }

        // Publish an integration event so other modules
        // can react to the newly issued ticket.
        await eventBus.PublishAsync(
            new TicketIssuedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.EventId,
                result.Value.Code),
            cancellationToken);
    }
}
