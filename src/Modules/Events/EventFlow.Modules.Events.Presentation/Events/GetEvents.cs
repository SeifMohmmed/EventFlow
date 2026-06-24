using EventFlow.Modules.Events.Application.Events.GetEvents;
using EventFlow.Modules.Events.Domain.Abstractions;
using EventFlow.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Events;

internal static class GetEvents
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events", async (ISender sender) =>
        {
            Result<IReadOnlyCollection<Application.Events.GetEvents.EventResponse>> result = await sender.Send(new GetEventsQuery());

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Events);
    }
}
