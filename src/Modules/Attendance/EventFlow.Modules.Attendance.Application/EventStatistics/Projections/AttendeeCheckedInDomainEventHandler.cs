using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Attendees;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Updates the event statistics projection when an attendee successfully checks in.
internal sealed class AttendeeCheckedInDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory,
    IEventStatisticsRepository eventStatisticsRepository)
    : DomainEventHandler<AttendeeCheckedInDomainEvent>
{
    public override async Task Handle(
        AttendeeCheckedInDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Open a database connection to query the latest attendance information.
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Count the number of tickets that have been used for the event.
        // Recalculating the value keeps the projection consistent even if
        // the event is processed more than once.
        const string sql =
            """
            SELECT COUNT(*)
            FROM attendance.tickets t
            WHERE
                t.event_id = es.event_id AND
                t.used_at_utc IS NOT NULL)
            """;

        int attendeeCount = await connection.ExecuteAsync(sql, domainEvent);

        // Load the current event statistics document.
        EventStatistics eventStatistics =
            await eventStatisticsRepository.GetAsync(domainEvent.EventId, cancellationToken);

        // Update the checked-in attendee count.
        eventStatistics.AttendeesCheckedIn = attendeeCount;

        // Persist the updated projection.
        await eventStatisticsRepository.ReplaceAsync(eventStatistics, cancellationToken);
    }
}
