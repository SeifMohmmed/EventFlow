using Microsoft.AspNetCore.Routing;

namespace EventFlow.Common.Presentation.Endpoints;

/// <summary>
/// Defines a contract for registering HTTP endpoints.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the application's routing pipeline.
    /// </summary>
    void MapEndpoint(IEndpointRouteBuilder app);
}
