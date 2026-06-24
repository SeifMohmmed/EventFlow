using EventFlow.Modules.Events.Application.Events.GetEvent;
using EventFlow.Modules.Events.Domain.Abstractions;
using EventFlow.Modules.Events.Presentation;
using EventFlow.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Api.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id}", async (Guid id, ISender sender) =>
        {
            Result<EventResponse> result = await sender.Send(new GetEventQuery(id));

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Events);
    }
}
