using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;

namespace EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

// Query handler responsible for reading the materialized Event Statistics view.
internal sealed class GetEventStatisticsQueryHandler(IEventStatisticsRepository eventStatisticsRepository)
    : IQueryHandler<GetEventStatisticsQuery, EventStatistics>
{
    public async Task<Result<EventStatistics>> Handle(
        GetEventStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        EventStatistics? eventStatistics =
            await eventStatisticsRepository.GetAsync(request.EventId, cancellationToken);

        return eventStatistics;
    }
}
