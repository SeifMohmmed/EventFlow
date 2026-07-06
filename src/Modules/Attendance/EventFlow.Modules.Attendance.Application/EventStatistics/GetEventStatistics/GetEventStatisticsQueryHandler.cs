using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Domain.Events;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

// Query handler responsible for reading the materialized Event Statistics view.
internal sealed class GetEventStatisticsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventStatisticsQuery, EventStatisticsResponse>
{
    public async Task<Result<EventStatisticsResponse>> Handle(
        GetEventStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        // Open a read-only database connection.
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Read data directly from the materialized projection instead of rebuilding
        // statistics from multiple tables.
        const string sql =
            $"""
             SELECT
                 event_id AS {nameof(EventStatisticsResponse.EventId)},
                 title AS {nameof(EventStatisticsResponse.Title)},
                 description AS {nameof(EventStatisticsResponse.Description)},
                 location AS {nameof(EventStatisticsResponse.Location)},
                 starts_at_utc AS {nameof(EventStatisticsResponse.StartsAtUtc)},
                 ends_at_utc AS {nameof(EventStatisticsResponse.EndsAtUtc)},
                 tickets_sold AS {nameof(EventStatisticsResponse.TicketsSold)},
                 attendees_checked_in AS {nameof(EventStatisticsResponse.AttendeesCheckedIn)},
                 duplicate_check_in_tickets AS {nameof(EventStatisticsResponse.DuplicateCheckInTickets)},
                 invalid_check_in_tickets AS {nameof(EventStatisticsResponse.InvalidCheckInTickets)}
             FROM attendance.event_statistics
             WHERE event_id = @EventId
             """;

        // Execute the query and map the result to the response DTO.
        EventStatisticsResponse? eventStatistics =
            await connection.QuerySingleOrDefaultAsync<EventStatisticsResponse>(sql, request);

        // Return a failure result if no projection exists.
        if (eventStatistics is null)
        {
            return Result.Failure<EventStatisticsResponse>(EventErrors.NotFound(request.EventId));
        }

        return eventStatistics;
    }
}
