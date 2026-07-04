using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
using EventFlow.Modules.Ticketing.Domain.Events;
using MediatR;

namespace EventFlow.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class ArchiveTicketsEventCanceledDomainEventHandler(ISender sender)
    : DomainEventHandler<EventCanceledDomainEvent>
{
    public override async Task Handle(
        EventCanceledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Archive all tickets associated with the cancelled event.
        Result result = await sender.Send(
            new ArchiveTicketsForEventCommand(domainEvent.EventId),
            cancellationToken);

        // Stop processing if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(ArchiveTicketsForEventCommand),
                result.Error);
        }
    }
}
