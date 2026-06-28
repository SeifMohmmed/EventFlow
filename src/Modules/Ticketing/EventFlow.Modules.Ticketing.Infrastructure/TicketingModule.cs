using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Ticketing.Application.Carts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    /// <summary>
    /// Registers the Ticketing module services.
    /// </summary>
    public static IServiceCollection AddTicketingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        // Discover and register the module's endpoints.
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    /// <summary>
    /// Registers the infrastructure services for the Ticketing module.
    /// </summary>
#pragma warning disable S1172 // Unused method parameters should be removed
#pragma warning disable IDE0060 // Unused method parameters should be removed
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.TryAddSingleton<CartService>();
    }
}
