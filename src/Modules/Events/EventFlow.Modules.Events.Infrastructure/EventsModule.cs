using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.Infrastructure.Categories;
using EventFlow.Modules.Events.Infrastructure.Database;
using EventFlow.Modules.Events.Infrastructure.Events;
using EventFlow.Modules.Events.Infrastructure.TicketTypes;
using EventFlow.Modules.Events.Presentation.Categories;
using EventFlow.Modules.Events.Presentation.Events;
using EventFlow.Modules.Events.Presentation.TicketTypes;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        TicketTypeEndpoints.MapEndpoints(app);
        CategoryEndpoints.MapEndpoints(app);
        EventEndpoints.MapEndpoints(app);
    }


    public static IServiceCollection AddEventModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(
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
                 .UseSnakeCaseNamingConvention()     // EventCategory → event_category
                 .AddInterceptors());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}
