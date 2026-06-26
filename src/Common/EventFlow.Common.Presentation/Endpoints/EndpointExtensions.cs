using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.Common.Presentation.Endpoints;

/// <summary>
/// Provides extension methods for discovering and mapping
/// application endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Discovers and registers all endpoint implementations
    /// from the specified assemblies.
    /// </summary>
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // Discover all concrete endpoint types and register them.
        ServiceDescriptor[] serviceDescriptors = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all registered endpoints to the application's routing pipeline.
    /// </summary>
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        // Resolve all registered endpoint implementations.
        IEnumerable<IEndpoint> endpoints =
            app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        // Map endpoints either to the application or to the specified route group.
        IEndpointRouteBuilder builder =
            routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}
