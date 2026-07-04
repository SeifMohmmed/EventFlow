namespace EventFlow.Common.Infrastructure.Outbox;

/// <summary>
/// Tracks which consumer has processed an outbox message.
/// </summary>
public sealed class OutboxMessageConsumer(Guid outboxMessageId, string name)
{
    // The processed outbox message.
    public Guid OutboxMessageId { get; init; } = outboxMessageId;

    // The name of the consumer that processed the message.
    public string Name { get; init; } = name;
}
