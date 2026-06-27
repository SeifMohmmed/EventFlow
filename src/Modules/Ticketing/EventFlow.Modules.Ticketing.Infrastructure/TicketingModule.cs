using EventFlow.Common.Presentation.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // will implement this later.
    }
}
