using EventFlow.Modules.Api.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Api.Events;

public static class CreateEvents
{
    public static void MapEnpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, EventsDbContext context) =>
        {
            var @event = new Event
            {
                Description = request.Description,
                Title = request.Title,
                Location = request.Location,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,

            };

            context.Events.Add(@event);

            await context.SaveChangesAsync();

            return Results.Ok(@event.Id);
        })
            .WithTags(Tages.Events);
    }

    internal sealed class Request
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartsAtUtc { get; set; }
        public DateTime? EndsAtUtc { get; set; }
    }
}
