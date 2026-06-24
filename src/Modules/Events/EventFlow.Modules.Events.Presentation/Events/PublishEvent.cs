using EventFlow.Modules.Events.Application.Events.PublishEvent;
using EventFlow.Modules.Events.Domain.Abstractions;
using EventFlow.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Events;

internal static class PublishEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id}/publish", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new PublishEventCommand(id));

            return result.Match(Results.NoContent, ApiResult.Problem);
        })
        .WithTags(Tags.Events);
    }
}
