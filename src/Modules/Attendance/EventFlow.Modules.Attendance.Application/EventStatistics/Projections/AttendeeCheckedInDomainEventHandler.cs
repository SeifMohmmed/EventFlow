using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Attendees;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Updates the projection whenever an attendee successfully checks in.
internal sealed class AttendeeCheckedInDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<AttendeeCheckedInDomainEvent>
{
    public override async Task Handle(
        AttendeeCheckedInDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Recalculate the number of checked-in attendees.
        // This guarantees consistency even if events are processed more than once.
        const string sql =
            """
            UPDATE attendance.event_statistics es
            SET attendees_checked_in = (
                SELECT COUNT(*)
                FROM attendance.tickets t
                WHERE
                    t.event_id = es.event_id AND
                    t.used_at_utc IS NOT NULL)
            WHERE es.event_id = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
