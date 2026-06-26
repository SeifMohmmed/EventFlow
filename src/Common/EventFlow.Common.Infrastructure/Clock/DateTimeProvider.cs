using EventFlow.Common.Application.Clock;

namespace EventFlow.Common.Infrastructure.Clock;

/// <summary>
/// Provides the current UTC date and time.
/// </summary>
internal sealed class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;
}
