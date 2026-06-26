namespace EventFlow.Common.Domain;

/// <summary>
/// Represents a domain event.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the event identifier.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the UTC time when the event occurred.
    /// </summary>
    DateTime OccurredOnUtc { get; }
}
