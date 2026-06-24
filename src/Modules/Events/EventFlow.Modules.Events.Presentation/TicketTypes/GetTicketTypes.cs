using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketTypes;
using EventFlow.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.TicketTypes;

internal static class GetTicketTypes
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket-types", async (Guid eventId, ISender sender) =>
        {
            Result<IReadOnlyCollection<TicketTypeResponse>> result = await sender.Send(
                new GetTicketTypesQuery(eventId));

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.TicketTypes);
    }
}
