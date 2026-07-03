namespace EventFlow.Common.Infrastructure.Outbox;

/// <summary>
/// Represents a domain event stored in the Outbox table.
/// </summary>
public sealed class OutboxMessage
{
    // Unique identifier of the original domain event.
    public Guid Id { get; init; }

    // CLR type name used to deserialize the event.
    public string Type { get; init; }

    // Serialized domain event payload.
    public string Content { get; init; }

    // Time when the domain event occurred.
    public DateTime OccurredOnUtc { get; init; }

    // Time when the message was successfully processed.
    public DateTime? ProcessedOnUtc { get; init; }

    // Error details if processing failed.
    public string? Error { get; init; }
}
