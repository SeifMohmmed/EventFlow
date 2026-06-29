namespace EventFlow.Common.Application.EventBus;

/// <summary>
/// Base class for all integration events exchanged
/// between application modules.
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Initializes a new integration event.
    /// </summary>
    protected IntegrationEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }

    /// <summary>
    /// Gets the unique identifier of the integration event.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the UTC time when the integration event occurred.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; }
}
