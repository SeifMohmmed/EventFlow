using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Tickets.GetTicket;
using EventFlow.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
using EventFlow.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class OrderTicketsIssuedDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderTicketsIssuedDomainEvent>
{
    public override async Task Handle(
        OrderTicketsIssuedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Retrieve all tickets created for the order.
        Result<IReadOnlyCollection<TicketResponse>> result =
            await sender.Send(
                new GetTicketsForOrderQuery(domainEvent.OrderId),
                cancellationToken);

        // Stop processing if the tickets cannot be retrieved.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(GetTicketsForOrderQuery),
                result.Error);
        }

        // Send ticket confirmation notification.
    }
}
