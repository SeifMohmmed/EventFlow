using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Events;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Creates the initial materialized view row when a new event is created.
internal sealed class EventCreatedDomainEventHandler(IEventStatisticsRepository eventStatisticsRepository)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Create the initial event statistics projection.
        await eventStatisticsRepository.InsertAsync(
            EventStatistics.Create(
                domainEvent.EventId,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.Location,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc),
            cancellationToken);
    }
}
