using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
using EventFlow.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateTicketsDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        // Generate tickets for the newly created order.
        Result result = await sender.Send(
            new CreateTicketBatchCommand(notification.OrderId),
            cancellationToken);

        // Stop processing if ticket creation failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(CreateTicketBatchCommand),
                result.Error);
        }
    }
}
