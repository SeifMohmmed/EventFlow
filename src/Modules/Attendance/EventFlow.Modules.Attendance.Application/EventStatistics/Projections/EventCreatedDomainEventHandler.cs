using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Events;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Creates the initial materialized view row when a new event is created.
internal sealed class EventCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Insert the projection with default statistics.
        const string sql =
            """
            INSERT INTO attendance.event_statistics(
                event_id,
                title,
                description,
                location,
                starts_at_utc,
                ends_at_utc,
                tickets_sold,
                attendees_checked_in,
                duplicate_check_in_tickets,
                invalid_check_in_tickets)
            VALUES (
                @EventId,
                @Title,
                @Description,
                @Location,
                @StartsAtUtc,
                @EndsAtUtc,
                @TicketsSold,
                @AttendeesCheckedIn,
                @DuplicateCheckInTickets,
                @InvalidCheckInTickets)
            """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                domainEvent.EventId,
                domainEvent.Title,
                domainEvent.Description,
                domainEvent.Location,
                domainEvent.StartsAtUtc,
                domainEvent.EndsAtUtc,

                // Initial projection state.
                TicketsSold = 0,
                AttendeesCheckedIn = 0,
                DuplicateCheckInTickets = Array.Empty<string>(),
                InvalidCheckInTickets = Array.Empty<string>()
            });
    }
}
