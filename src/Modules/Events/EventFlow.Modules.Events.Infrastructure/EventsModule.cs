using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.Infrastructure.Data;
using EventFlow.Modules.Events.Infrastructure.Database;
using EventFlow.Modules.Events.Infrastructure.Events;
using EventFlow.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace EventFlow.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }


    public static IServiceCollection AddEventModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database")!;

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

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

        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

    }
}
