using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Attendance.Domain.Tickets;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.Projections;

// Updates ticket sales statistics whenever a ticket is created.
internal sealed class TicketCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        // Recalculate total tickets sold for the event.
        const string sql =
            """
            UPDATE attendance.event_statistics es
            SET tickets_sold = (
                SELECT COUNT(*)
                FROM attendance.tickets t
                WHERE t.event_id = es.event_id)
            WHERE es.event_id = @EventId
            """;

        await connection.ExecuteAsync(sql, domainEvent);
    }
}
