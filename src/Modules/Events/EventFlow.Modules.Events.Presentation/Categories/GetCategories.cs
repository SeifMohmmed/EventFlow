using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Common.Presentation.Results;
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
        app.MapGet("categories", async (ISender sender) =>
        {
            // Fetch the categories from the application layer.
            Result<IReadOnlyCollection<CategoryResponse>> result =
                await sender.Send(new GetCategoriesQuery());

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
