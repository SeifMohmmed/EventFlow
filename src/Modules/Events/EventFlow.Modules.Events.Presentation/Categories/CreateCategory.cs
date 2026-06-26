using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.ApiResults;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Events.Application.Categories.CreateCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Categories;

internal sealed class CreateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (Request request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateCategoryCommand(request.Name));

            return result.Match(Results.Ok, ApiResult.Problem);
        })
        .WithTags(Tags.Categories);
    }

    internal sealed class Request
    {
        public string Name { get; init; }
    }
}
