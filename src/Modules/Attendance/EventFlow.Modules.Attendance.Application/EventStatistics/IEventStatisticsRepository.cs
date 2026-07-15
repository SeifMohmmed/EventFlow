namespace EventFlow.Modules.Attendance.Application.EventStatistics;

// Defines persistence operations for the Event Statistics projection.
public interface IEventStatisticsRepository
{
    // Retrieves the statistics for a specific event.
    Task<EventStatistics> GetAsync(Guid eventId, CancellationToken cancellationToken = default);

    // Inserts a new event statistics projection.
    Task InsertAsync(EventStatistics eventStatistics, CancellationToken cancellationToken = default);

    // Replaces an existing event statistics projection.
    Task ReplaceAsync(EventStatistics eventStatistics, CancellationToken cancellationToken = default);
}
