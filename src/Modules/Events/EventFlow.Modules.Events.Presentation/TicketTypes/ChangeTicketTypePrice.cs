using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
using EventFlow.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.TicketTypes;


internal sealed class ChangeTicketTypePrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("ticket-types/{id}/price", async (Guid id, Request request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));

            return result.Match(Results.NoContent, ApiResult.Problem);
        })
          .RequireAuthorization(Permissions.ModifyTicketTypes)
          .WithTags(Tags.TicketTypes);
    }

    internal sealed class Request
    {
        public decimal Price { get; init; }
    }
}
