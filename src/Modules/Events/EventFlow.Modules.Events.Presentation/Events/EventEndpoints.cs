using EventFlow.Modules.Api.Events;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        GetEvent.MapEndpoint(app);
        CreateEvents.MapEnpoint(app);
    }
}
