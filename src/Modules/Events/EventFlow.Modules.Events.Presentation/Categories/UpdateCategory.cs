using EventFlow.Common.Domain;
using EventFlow.Common.Presentation.ApiResults;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Events.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace EventFlow.Modules.Events.Presentation.Categories;

internal sealed class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}", async (Guid id, Request request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateCategoryCommand(id, request.Name));

            return result.Match(() => Results.Ok(), ApiResult.Problem);
        })
        .WithTags(Tags.Categories);
    }

    internal sealed class Request
    {
        public string Name { get; init; }
    }
}
