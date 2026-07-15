using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Tickets;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Updates ticket sales statistics whenever a ticket is created.
internal sealed class TicketCreatedDomainEventHandler(
    IDbConnectionFactory dbConnectionFactory,
    IEventStatisticsRepository eventStatisticsRepository)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Open a database connection to query the latest ticket information.
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Recalculate the total number of tickets sold for the event.
        const string sql =
            """
            SELECT COUNT(*)
            FROM attendance.tickets t
            WHERE t.event_id = @EventId
            """;

        int ticketCount = await connection.ExecuteScalarAsync<int>(sql, domainEvent);

        // Load the event statistics projection.
        EventStatistics eventStatistics =
        await eventStatisticsRepository.GetAsync(domainEvent.EventId, cancellationToken);

        // Update the ticket sales count.
        eventStatistics.TicketsSold = ticketCount;

        // Persist the updated projection.
        await eventStatisticsRepository.ReplaceAsync(eventStatistics, cancellationToken);
    }
}
