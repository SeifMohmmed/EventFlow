using EventFlow.Common.Application.Clock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace EventFlow.Modules.Events.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Configures the application host and infrastructure used during integration tests.
public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Mock date/time provider used to control time-dependent behavior.
    public readonly IDateTimeProvider DateTimeProviderMock =
        Substitute.For<IDateTimeProvider>();

    // PostgreSQL database used by the tests.
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder("postgres:17")
            .WithDatabase("eventFlow")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    // Redis instance used by the application.
    private readonly RedisContainer _redisContainer =
        new RedisBuilder("redis:latest")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configure infrastructure connection strings.
        Environment.SetEnvironmentVariable(
            "ConnectionStrings:Database",
            _dbContainer.GetConnectionString());

        Environment.SetEnvironmentVariable(
            "ConnectionStrings:Cache",
            _redisContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            // Replace the production clock with a mock implementation.
            services.RemoveAll<IDateTimeProvider>();

            DateTimeProviderMock.UtcNow.Returns(_ => DateTime.UtcNow);

            services.AddSingleton(DateTimeProviderMock);
        });
    }

    public async Task InitializeAsync()
    {
        // Start all infrastructure containers.
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        // Stop all infrastructure containers.
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}
