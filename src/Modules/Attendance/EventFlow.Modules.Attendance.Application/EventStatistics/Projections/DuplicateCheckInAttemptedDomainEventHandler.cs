using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Attendees;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Records duplicate ticket scan attempts inside the projection.
internal sealed class DuplicateCheckInAttemptedDomainEventHandler(IEventStatisticsRepository eventStatisticsRepository)
    : DomainEventHandler<DuplicateCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        DuplicateCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Load the event statistics projection.
        EventStatistics eventStatistics =
            await eventStatisticsRepository.GetAsync(domainEvent.EventId, cancellationToken);

        // Record the duplicate ticket code.
        eventStatistics.DuplicateCheckInTickets.Add(domainEvent.TicketCode);

        // Persist the updated projection.
        await eventStatisticsRepository.ReplaceAsync(eventStatistics, cancellationToken);
    }
}
