using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Infrastructure.Outbox;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Users.Application.Abstractions.Data;
using EventFlow.Modules.Users.Application.Abstractions.Identity;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure.Authorization;
using EventFlow.Modules.Users.Infrastructure.Database;
using EventFlow.Modules.Users.Infrastructure.Identity;
using EventFlow.Modules.Users.Infrastructure.Outbox;
using EventFlow.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace EventFlow.Modules.Users.Infrastructure;

/// <summary>
/// Configures the Users module services and endpoints.
/// </summary>
public static class UsersModule
{
    /// <summary>
    /// Registers the Users module services.
    /// </summary>
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        // Discover and register the module's endpoints.
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    /// <summary>
    /// Registers the infrastructure services for the Users module.
    /// </summary>
    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPermissionService, PermissionService>();

        // Bind Keycloak settings from configuration.
        services.Configure<KeyCloakOptions>(
            configuration.GetSection("Users:KeyCloak"));

        // Handler that attaches an admin access token to Keycloak requests.
        services.AddTransient<KeyCloakAuthDelegatingHandler>();

        // Configure the HTTP client used to communicate with the Keycloak Admin API.
        services
            .AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
            {
                KeyCloakOptions keyCloakOptions = serviceProvider
                    .GetRequiredService<IOptions<KeyCloakOptions>>()
                    .Value;

                // Set the Keycloak Admin API base address.
                httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
            })
            // Add the authentication handler to outgoing requests.
            .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        // Register the service responsible for identity provider operations.
        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        // Register the Users module DbContext and configure PostgreSQL,
        // migration history, domain event publishing, and snake_case naming.
        services.AddDbContext<UsersDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(
                            HistoryRepository.DefaultTableName,
                            Schemas.Users))
                // Publish domain events after a successful SaveChanges call.
                .AddInterceptors(
                    sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
                // Convert table and column names to snake_case.
                .UseSnakeCaseNamingConvention());

        // Register repositories.
        services.AddScoped<IUserRepository, UserRepository>();

        // Register the Unit of Work implementation.
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<UsersDbContext>());

        // Bind Users module outbox settings from configuration.
        services.Configure<OutboxOptions>(
            configuration.GetSection("Users:Outbox"));

        // Register Quartz configuration for the outbox processing job.
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }
}
