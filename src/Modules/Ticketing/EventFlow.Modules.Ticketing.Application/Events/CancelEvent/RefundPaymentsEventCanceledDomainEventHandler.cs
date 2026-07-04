using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;
using EventFlow.Modules.Ticketing.Domain.Events;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class RefundPaymentsEventCanceledDomainEventHandler(ISender sender)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Refund all payments associated with the cancelled event.
        Result result = await sender.Send(
            new RefundPaymentsForEventCommand(domainEvent.EventId),
            cancellationToken);

        // Stop processing if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(RefundPaymentsForEventCommand),
                result.Error);
        }
    }
}
