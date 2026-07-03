using EventFlow.Common.Domain;
using EventFlow.Common.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace EventFlow.Common.Infrastructure.Outbox;

/// <summary>
/// EF Core interceptor that converts domain events into
/// outbox messages before the transaction is committed.
/// </summary>
public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        // Insert outbox messages only when a DbContext is available.
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return await base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    /// <summary>
    /// Collects domain events from tracked entities and stores
    /// them as outbox messages in the current transaction.
    /// </summary>
    private static void InsertOutboxMessages(DbContext context)
    {
        // Retrieve all tracked domain entities.
        var outboxMessages = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)

            // Collect each entity's domain events and clear them
            // so they won't be processed again.
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents =
                    entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })

            // Convert each domain event into an outbox message.
            .Select(domainEvent => new OutboxMessage
            {
                // Preserve the original event identifier.
                Id = domainEvent.Id,

                // Store the event type for later deserialization.
                Type = domainEvent.GetType().Name,

                // Serialize the event payload.
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    SerializerSettings.Instance),

                // Preserve when the event originally occurred.
                OccurredOnUtc = domainEvent.OccurredOnUtc
            })
            .ToList();

        // Save all outbox messages as part of the current transaction.
        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
