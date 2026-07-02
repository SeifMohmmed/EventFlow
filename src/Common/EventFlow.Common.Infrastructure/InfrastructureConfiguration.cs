using EventFlow.Common.Application.Caching;
using EventFlow.Common.Application.Clock;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Infrastructure.Authentication;
using EventFlow.Common.Infrastructure.Authorization;
using EventFlow.Common.Infrastructure.Caching;
using EventFlow.Common.Infrastructure.Clock;
using EventFlow.Common.Infrastructure.Data;
using EventFlow.Common.Infrastructure.Interceptors;
using MassTransit;
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
        Action<IRegistrationConfigurator>[] modelConfigureConsumers,
        string databaseConnectionString,
        string redisConnectionString)
    {
        // Register authentication
        services.AddAuthenticationInternal();

        // Register authorization
        services.AddAuthorizationInternal();

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

        // Register the application's event bus abstraction.
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        // Configure MassTransit and register message consumers.
        services.AddMassTransit(cfg =>
        {
            // Register consumers from each module.
            foreach (Action<IRegistrationConfigurator> configureConsumer in modelConfigureConsumers)
            {
                configureConsumer(cfg);
            }

            // Use kebab-case naming for generated endpoints.
            cfg.SetKebabCaseEndpointNameFormatter();

            // Configure the in-memory transport.
            cfg.UsingInMemory((context, configure) =>
            {
                // Automatically configure endpoints for all registered consumers.
                configure.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
