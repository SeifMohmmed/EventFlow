namespace EventFlow.Modules.Users.Infrastructure.Outbox;

/// <summary>
/// Configuration options for outbox processing.
/// </summary>
internal sealed class OutboxOptions
{
    // Interval between job executions.
    public int IntervalInSeconds { get; init; }

    // Maximum number of messages processed per execution.
    public int BatchSize { get; init; }
}
