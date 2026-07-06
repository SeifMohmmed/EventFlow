using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
using EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Attendance.Presentation.EventStatistics;

// Endpoint for retrieving the materialized statistics of an event.
internal sealed class GetEventStatistics : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("event-statistics/{id}", async (Guid id, ISender sender) =>
        {
            // Execute the query through MediatR.
            Result<EventStatisticsResponse> result =
                await sender.Send(new GetEventStatisticsQuery(id));

            // Return either the statistics or a standardized error response.
            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .RequireAuthorization(Permissions.GetEventStatistics)
        .WithTags(Tags.EventStatistics);
    }
}
