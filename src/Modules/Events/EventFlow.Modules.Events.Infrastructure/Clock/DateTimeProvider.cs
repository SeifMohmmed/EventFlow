using EventFlow.Modules.Events.Application.Abstractions.Clock;

namespace EventFlow.Modules.Events.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
