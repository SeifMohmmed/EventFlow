using EventFlow.Common.Application.Caching;
using EventFlow.Common.Application.Clock;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Infrastructure.Caching;
using EventFlow.Common.Infrastructure.Clock;
using EventFlow.Common.Infrastructure.Data;
using EventFlow.Common.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace EventFlow.Common.Infrastructure;

/// <summary>
/// Provides extension methods for registering
/// infrastructure services.
/// </summary>
public static class InfrastructureConfiguration
{
    /// <summary>
    /// Registers infrastructure services, including database access,
    /// distributed caching, and application services.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseConnectionString,
        string redisConnectionString)
    {
        // Create and register the PostgreSQL data source.
        NpgsqlDataSource npgsqlDataSource =
            new NpgsqlDataSourceBuilder(databaseConnectionString).Build();

        services.TryAddSingleton(npgsqlDataSource);

        // Register the database connection factory.
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        // Register the application's date/time provider.
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        try
        {
            // During EF Core design-time operations (e.g. Add-Migration),
            // the application's Program.cs is executed to build the service provider.
            // If Redis is unavailable, creating the connection would throw an exception
            // and prevent migrations from being generated.

            // Create and register the Redis connection multiplexer.
            IConnectionMultiplexer connectionMultiplexer =
                ConnectionMultiplexer.Connect(redisConnectionString);

            services.TryAddSingleton(connectionMultiplexer);

            // Configure the distributed cache to use Redis.
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory =
                    () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {
            // Fall back to an in-memory distributed cache when Redis is unavailable.
            // This allows the application (especially EF Core migrations) to start
            // without requiring a running Redis instance.
            services.AddDistributedMemoryCache();
        }

        // Register the application's cache service.
        services.TryAddSingleton<ICacheService, CacheService>();

        return services;
    }
}
