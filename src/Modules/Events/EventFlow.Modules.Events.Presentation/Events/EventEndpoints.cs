using EventFlow.Modules.Api.Events;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        CancelEvent.MapEndpoint(app);
        CreateEvent.MapEndpoint(app);
        GetEvent.MapEndpoint(app);
        GetEvents.MapEndpoint(app);
        PublishEvent.MapEndpoint(app);
        RescheduleEvent.MapEndpoint(app);
        SearchEvents.MapEndpoint(app);
    }
}
