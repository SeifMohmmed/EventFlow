namespace EventFlow.Common.Application.EventBus;

/// <summary>
/// Represents an integration event published
/// to other application modules.
/// </summary>
public interface IIntegrationEvent
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
