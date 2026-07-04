using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Common.Infrastructure.Outbox;

namespace EventFlow.Modules.Users.Infrastructure.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(
        TDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Open a database connection.
        await using DbConnection connection =
            await dbConnectionFactory.OpenConnectionAsync();

        // Identify this consumer for the current outbox message.
        var outboxMessageConsumer =
            new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        // Skip processing if this consumer has already handled the message.
        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
        {
            return;
        }

        // Execute the decorated domain event handler.
        await decorated.Handle(domainEvent, cancellationToken);

        // Record that this consumer has successfully processed the message.
        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }

    private static async Task<bool> OutboxConsumerExistsAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer)
    {
        // Check whether this consumer has already processed the message.
        const string sql =
            """
            SELECT EXISTS(
                SELECT 1
                FROM users.outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND
                      name = @Name
            )
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(
            sql,
            outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer)
    {
        // Record successful message processing.
        const string sql =
            """
            INSERT INTO users.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
