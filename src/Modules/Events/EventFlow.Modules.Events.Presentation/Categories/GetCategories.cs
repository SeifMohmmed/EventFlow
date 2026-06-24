using EventFlow.Modules.Events.Application.Categories.GetCategories;
using EventFlow.Modules.Events.Application.Categories.GetCategory;
using EventFlow.Modules.Events.Domain.Abstractions;
using EventFlow.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Categories;

internal static class GetCategories
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender) =>
        {
            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
