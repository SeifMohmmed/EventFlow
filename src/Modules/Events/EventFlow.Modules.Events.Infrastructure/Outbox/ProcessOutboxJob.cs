using System.Data;
using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Clock;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Common.Infrastructure.Outbox;
using EventFlow.Common.Infrastructure.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace EventFlow.Modules.Events.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = "Events";

    public async Task Execute(IJobExecutionContext context)
    {
#pragma warning disable CA1873
        //1. Get unprocessed outbox messages from the database
        logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);

        // Open a database connection and begin a transaction.
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        // Load a batch of unprocessed outbox messages.
        IReadOnlyList<OutboxMessageResponse> outboxMessages =
            await GetOutboxMessagesAsync(connection, transaction);

        //2. Iterate through each outbox message, deserialize it, and publish it using MediatR
        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                // Deserialize the stored domain event.
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    SerializerSettings.Instance)!;

                // Resolve MediatR from a new DI scope.
                using IServiceScope scope = serviceScopeFactory.CreateScope();

                // Resolve domain event handlers from the current module.
                IEnumerable<IDomainEventHandler> domainEventHandlers =
                    DomainEventHandlersFactory.GetHandlers(
                        domainEvent.GetType(),
                        scope.ServiceProvider,
                        Application.AssemblyReference.Assembly);

                foreach (IDomainEventHandler domainEventHandler in domainEventHandlers)
                {
                    // Invoke each handler for the domain event.
                    await domainEventHandler.Handle(domainEvent);
                }
            }
            catch (Exception caughtException)
            {
                // Record the failure and continue processing the remaining messages.
                logger.LogError(
                    caughtException,
                    "{Module} - Exception while processing outbox message {MessageId}",
                    ModuleName,
                    outboxMessage.Id);

                exception = caughtException;
            }
            //3. Update the outbox message as processed, including any error information if an exception occurred
            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        // Commit all message updates.
        await transaction.CommitAsync();
#pragma warning disable CA1873
        logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        // Select the next batch of unprocessed outbox messages.
        // Lock the selected rows to prevent multiple workers
        // from processing the same messages simultaneously.
        string sql =
            $"""
             SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
             FROM events.outbox_messages
             WHERE processed_on_utc IS NULL
             ORDER BY occurred_on_utc
             LIMIT {outboxOptions.Value.BatchSize}
             FOR UPDATE
             """;

        // Execute the query and map the results.
        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
            sql,
            transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        // Mark the message as processed and record any processing error.
        const string sql =
            """
            UPDATE events.outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id
            """;

        // Update the processing status.
        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
    }

    /// <summary>
    /// Represents an outbox message loaded for processing.
    /// </summary>
    internal sealed record OutboxMessageResponse(
        Guid Id,
        string Content);
}
