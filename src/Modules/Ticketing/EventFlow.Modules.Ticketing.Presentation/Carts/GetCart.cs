using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
using EventFlow.Modules.Ticketing.Application.Abstractions.Authentication;
using EventFlow.Modules.Ticketing.Application.Carts;
using EventFlow.Modules.Ticketing.Application.Carts.GetCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Ticketing.Presentation.Carts;

internal sealed class GetCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("carts", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result<Cart> result = await sender.Send(new GetCartQuery(customerContext.CustomerId));

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .RequireAuthorization(Permissions.GetCart)
        .WithTags(Tags.Carts);
    }
}
