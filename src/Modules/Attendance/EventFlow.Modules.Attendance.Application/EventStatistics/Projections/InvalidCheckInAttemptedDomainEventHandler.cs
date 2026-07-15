using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Attendees;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Stores invalid ticket scan attempts in the projection.
internal sealed class InvalidCheckInAttemptedDomainEventHandler(IEventStatisticsRepository eventStatisticsRepository)
    : DomainEventHandler<InvalidCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        InvalidCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Load the event statistics projection.
        EventStatistics eventStatistics =
            await eventStatisticsRepository.GetAsync(domainEvent.EventId, cancellationToken);

        // Record the invalid ticket code.
        eventStatistics.InvalidCheckInTickets.Add(domainEvent.TicketCode);

        // Persist the updated projection.
        await eventStatisticsRepository.ReplaceAsync(eventStatistics, cancellationToken);
    }
}
