using EventFlow.Modules.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Api.Events;

public static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, EventsDbContext context) =>
        {
            EventResponse @event = await context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventResponse(
                    e.Id,
                    e.Title,
                    e.Description,
                    e.Location,
                    e.StartsAtUtc,
                    e.EndsAtUtc))
                .SingleOrDefaultAsync();

            return @event is null ? Results.NotFound() : Results.Ok(@event);
        }).WithTags(Tages.Events);

    }
}
