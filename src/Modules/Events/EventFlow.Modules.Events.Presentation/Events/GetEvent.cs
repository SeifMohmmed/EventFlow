using EventFlow.Modules.Events.Application;
using EventFlow.Modules.Events.Application.Events.GetEvent;
using EventFlow.Modules.Events.Presentation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Api.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, ISender sender) =>
        {
            EventResponse @event = await sender.Send(new GetEventQuery(id));

            return @event is null ? Results.NotFound() : Results.Ok(@event);
        }).WithTags(Tages.Events);

    }
}
