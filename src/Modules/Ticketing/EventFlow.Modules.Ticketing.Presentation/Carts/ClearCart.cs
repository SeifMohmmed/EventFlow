using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
using EventFlow.Modules.Ticketing.Application.Abstractions.Authentication;
using EventFlow.Modules.Ticketing.Application.Carts.ClearCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Ticketing.Presentation.Carts;

internal sealed class ClearCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("carts", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result result = await sender.Send(new ClearCartCommand(customerContext.CustomerId));

            return result.Match(() => Results.Ok(), ApiResult.Problem);
        })
        .RequireAuthorization(Permissions.RemoveFromCart)
        .WithTags(Tags.Carts);
    }
}
