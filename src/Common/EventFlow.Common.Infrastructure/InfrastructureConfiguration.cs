using EventFlow.Common.Application.Caching;
using EventFlow.Common.Application.Clock;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Infrastructure.Authentication;
using EventFlow.Common.Infrastructure.Authorization;
using EventFlow.Common.Infrastructure.Caching;
using EventFlow.Common.Infrastructure.Clock;
using EventFlow.Common.Infrastructure.Data;
using EventFlow.Common.Infrastructure.EventBus;
using EventFlow.Common.Infrastructure.Outbox;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
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
        string serviceName,
        Action<IRegistrationConfigurator, string>[] modelConfigureConsumers,
        RabbitMqSettings rabbitMqSettings,
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

        // Register the EF Core interceptor that creates outbox messages.
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        // Register Quartz services.
        services.AddQuartz(configurator =>
        {
            // Generate unique Quartz scheduler IDs for test isolation
            var scheduler = Guid.CreateVersion7();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        // Run Quartz as a hosted background service.
        services.AddQuartzHostedService(options =>
            options.WaitForJobsToComplete = true);

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
            // Generate a unique instance identifier for this service.
            // Example: EventFlow.Api -> eventflow-api
            string instanceId = serviceName.ToLowerInvariant().Replace('.', '-');

            // Register consumers from each module.
            foreach (Action<IRegistrationConfigurator, string> configureConsumer in modelConfigureConsumers)
            {
                configureConsumer(cfg, instanceId);
            }

            // Use kebab-case naming for generated endpoints.
            cfg.SetKebabCaseEndpointNameFormatter();

            // Configure the RabbitMQ transport.
            cfg.UsingRabbitMq((context, configure) =>
            {
                // Configure the RabbitMQ host and credentials.
                configure.Host(new Uri(rabbitMqSettings.Host), h =>
                {
                    h.Username(rabbitMqSettings.UserName);
                    h.Password(rabbitMqSettings.Password);
                });

                // Automatically configure endpoints for all registered consumers.
                configure.ConfigureEndpoints(context);
            });
        });

        // Configure OpenTelemetry.
        services
            .AddOpenTelemetry()
            // Configure the service information included in telemetry.
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    // Capture incoming ASP.NET Core requests.
                    .AddAspNetCoreInstrumentation()
                    // Capture outgoing HTTP requests.
                    .AddHttpClientInstrumentation()
                    // Capture Entity Framework Core database operations.
                    .AddEntityFrameworkCoreInstrumentation()
                    // Capture PostgreSQL database commands.
                    .AddNpgsql()
                    // Capture MassTransit messaging activities.
                    .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

                // Export traces using the OTLP protocol.
                tracing.AddOtlpExporter();
            });

        return services;
    }
}
