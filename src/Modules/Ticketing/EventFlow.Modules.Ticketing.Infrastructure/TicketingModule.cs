using EventFlow.Common.Infrastructure.Interceptors;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Ticketing.Application.Abstractions.Data;
using EventFlow.Modules.Ticketing.Application.Carts;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Infrastructure.Customers;
using EventFlow.Modules.Ticketing.Infrastructure.Database;
using EventFlow.Modules.Ticketing.Presentation.Customers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
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
    /// Registers the Ticketing module's message consumers.
    /// </summary>
    public static void ConfigureConsumers(
        IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

    /// <summary>
    /// Registers the infrastructure services for the Ticketing module.
    /// </summary>
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the Ticketing module DbContext and configure PostgreSQL,
        // migration history, domain event publishing, and snake_case naming.
        services.AddDbContext<TicketingDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(
                            HistoryRepository.DefaultTableName,
                            Schemas.Ticketing))
                // Publish domain events after a successful SaveChanges call.
                .AddInterceptors(
                    sp.GetRequiredService<PublishDomainEventsInterceptor>())
                // Convert table and column names to snake_case.
                .UseSnakeCaseNamingConvention());

        // Register repositories.
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Register the Unit of Work implementation.
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<TicketingDbContext>());

        // Register the in-memory shopping cart service.
        services.AddSingleton<CartService>();
    }
}
