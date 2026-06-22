using EventFlow.Modules.Api.Database;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Api.Events;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvents.MapEnpoint(app);
        GetEvent.MapEndpoint(app);
    }


    public static IServiceCollection AddEventModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        string connectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions

                    // Configure EF Core migration history table location.
                    // By default EF creates: public.__EFMigrationsHistory
                    // Here we move it into the "events" schema so this module
                    // keeps its migrations isolated from other modules.
                    .MigrationsHistoryTable(
                        HistoryRepository.DefaultTableName, // "__EFMigrationsHistory"
                        Schemas.Events))                    // Schema: "events"
                 .UseSnakeCaseNamingConvention());          // EventCategory → event_category

        return services;
    }
}
