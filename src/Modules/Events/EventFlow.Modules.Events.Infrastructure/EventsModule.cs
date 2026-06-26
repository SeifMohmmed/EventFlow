using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.Infrastructure.Categories;
using EventFlow.Modules.Events.Infrastructure.Database;
using EventFlow.Modules.Events.Infrastructure.Events;
using EventFlow.Modules.Events.Infrastructure.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Events.Infrastructure;

/// <summary>
/// Configures the Events module services and endpoints.
/// </summary>
public static class EventsModule
{
    /// <summary>
    /// Registers the Events module services.
    /// </summary>
    public static IServiceCollection AddEventModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }

    /// <summary>
    /// Registers the infrastructure services for the Events module.
    /// </summary>
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString =
            configuration.GetConnectionString("Database")!;

        // Register the Events module DbContext.
        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions

                    // Store EF Core migration history inside the
                    // Events schema instead of the default "public" schema.
                    .MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName,
                        Schemas.Events))

                // Convert table and column names to snake_case.
                .UseSnakeCaseNamingConvention()

                // Register EF Core interceptors.
                .AddInterceptors());

        // Register the Unit of Work implementation.
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<EventsDbContext>());

        // Register repositories.
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}
