using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Domain.Tickets;
using EventFlow.Modules.Ticketing.IntegrationEvents;

namespace EventFlow.Modules.Ticketing.Application.Tickets.ArchiveTicket;

internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketArchivedDomainEvent>
{
    public override async Task Handle(
        TicketArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Publish an integration event so other modules
        // know the ticket has been archived.
        await eventBus.PublishAsync(
            new TicketArchivedIntegrationEvent(
                domainEvent.Id,
                domainEvent.OccurredOnUtc,
                domainEvent.TicketId,
                domainEvent.Code),
            cancellationToken);
    }
}
