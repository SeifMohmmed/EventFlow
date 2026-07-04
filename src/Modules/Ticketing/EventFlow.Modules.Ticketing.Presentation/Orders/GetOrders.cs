using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
using EventFlow.Modules.Ticketing.Application.Abstractions.Authentication;
using EventFlow.Modules.Ticketing.Application.Orders.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (ICustomerContext customerContext, ISender sender) =>
        {
            Result<IReadOnlyCollection<OrderResponse>> result = await sender.Send(
                new GetOrdersQuery(customerContext.CustomerId));

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .RequireAuthorization(Permissions.GetOrders)
        .WithTags(Tags.Orders);
    }
}
