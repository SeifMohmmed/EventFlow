using EventFlow.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace EventFlow.Modules.Users.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Configures the application host and infrastructure used during integration tests.
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // PostgreSQL database used by the tests.
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("eventFlow")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    // Redis instance used by the application.
    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:latest")
        .Build();

    // Keycloak instance preloaded with the test realm.
    private readonly KeycloakContainer _keycloakContainer =
        new KeycloakBuilder("quay.io/keycloak/keycloak:24.0.0")
            .WithResourceMapping(
                new FileInfo("eventFlow-realm-export.json"),
                new FileInfo("/opt/keycloak/data/import/realm.json"))
            .WithCommand("--import-realm")
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

        string keycloakAddress = _keycloakContainer.GetBaseAddress();
        string keyCloakRealmUrl = $"{keycloakAddress}realms/eventFlow";

        // Configure JWT authentication to use the test Keycloak instance.
        Environment.SetEnvironmentVariable(
            "Authentication:MetadataAddress",
            $"{keyCloakRealmUrl}/.well-known/openid-configuration");

        Environment.SetEnvironmentVariable(
            "Authentication:TokenValidationParameters:ValidIssuer",
            keyCloakRealmUrl);

        builder.ConfigureTestServices(services =>
        {
            // Override Keycloak endpoints for integration testing.
            services.Configure<KeyCloakOptions>(o =>
            {
                o.AdminUrl = $"{keycloakAddress}admin/realms/eventFlow/";
                o.TokenUrl = $"{keyCloakRealmUrl}/protocol/openid-connect/token";
            });
        });
    }

    public async Task InitializeAsync()
    {
        // Start all infrastructure containers before running tests.
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        // Stop all containers after the test suite finishes.
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
        await _keycloakContainer.StopAsync();
    }
}
