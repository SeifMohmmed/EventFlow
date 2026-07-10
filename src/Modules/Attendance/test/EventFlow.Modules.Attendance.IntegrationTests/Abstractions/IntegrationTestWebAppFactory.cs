using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace EventFlow.Modules.Attendance.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Configures the application host and infrastructure used during integration tests.
public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
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
        // Configure application connection strings.
        Environment.SetEnvironmentVariable(
            "ConnectionStrings:Database",
            _dbContainer.GetConnectionString());

        Environment.SetEnvironmentVariable(
            "ConnectionStrings:Cache",
            _redisContainer.GetConnectionString());
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
