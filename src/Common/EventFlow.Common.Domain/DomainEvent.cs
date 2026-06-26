namespace EventFlow.Common.Domain;

/// <summary>
/// Base class for all domain events.
/// Stores the event identifier and the time it occurred.
/// </summary>
public abstract class DomainEvent : IDomainEvent
{
    /// <summary>
    /// Initializes a new domain event with a generated identifier
    /// and the current UTC timestamp.
    /// </summary>
    protected DomainEvent()
    {
        Id = Guid.CreateVersion7();
        OccurredOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new domain event with the specified identifier
    /// and occurrence time.
    /// </summary>
    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }

    /// <summary>
    /// Gets the unique identifier of the domain event.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the UTC time when the event occurred.
    /// </summary>
    public DateTime OccurredOnUtc { get; init; }
}
