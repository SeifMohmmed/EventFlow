using EventFlow.Common.Application.Caching;
using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.ApiResults;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Events.Application.Categories.GetCategories;
using EventFlow.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            // Try to retrieve the categories from the cache.
            IReadOnlyCollection<CategoryResponse> categoryResponses =
                await cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>("categories");

            // Return the cached response if available.
            if (categoryResponses is not null)
            {
                return Results.Ok(categoryResponses);
            }

            // Fetch the categories from the application layer.
            Result<IReadOnlyCollection<CategoryResponse>> result =
                await sender.Send(new GetCategoriesQuery());

            // Cache the successful result for future requests.
            if (result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value);
            }

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
