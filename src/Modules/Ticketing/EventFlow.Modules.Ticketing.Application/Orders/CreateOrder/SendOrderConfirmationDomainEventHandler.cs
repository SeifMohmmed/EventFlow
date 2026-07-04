using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Orders.GetOrder;
using EventFlow.Modules.Ticketing.Domain.Orders;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderCreatedDomainEvent>
{
    public override async Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the order details.
        Result<OrderResponse> result = await sender.Send(
            new GetOrderQuery(notification.OrderId),
            cancellationToken);

        // Stop processing if the order cannot be found.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(GetOrderQuery),
                result.Error);
        }

        // Send order confirmation notification.
    }
}
