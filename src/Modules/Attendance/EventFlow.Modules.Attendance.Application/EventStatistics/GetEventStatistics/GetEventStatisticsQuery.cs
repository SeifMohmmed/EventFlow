using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

// CQRS query used to retrieve statistics for a specific event.
public sealed record GetEventStatisticsQuery(Guid EventId)
    : IQuery<EventStatistics>;
