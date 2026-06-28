using EventFlow.Common.Infrastructure.Interceptors;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Users.Application.Abstractions.Data;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure.Database;
using EventFlow.Modules.Users.Infrastructure.PublicApi;
using EventFlow.Modules.Users.Infrastructure.Users;
using EventFlow.Modules.Users.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    sp.GetRequiredService<PublishDomainEventsInterceptor>())
                // Convert table and column names to snake_case.
                .UseSnakeCaseNamingConvention());

        // Register repositories.
        services.AddScoped<IUserRepository, UserRepository>();

        // Register the Unit of Work implementation.
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IUsersApi, UsersApi>();
    }
}
