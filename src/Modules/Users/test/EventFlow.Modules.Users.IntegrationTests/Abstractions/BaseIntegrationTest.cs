using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Bogus;
using EventFlow.Modules.Users.Infrastructure.Database;
using EventFlow.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventFlow.Modules.Users.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Base class for integration tests providing shared infrastructure and helper methods.
[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest : IDisposable
{
    // Shared Faker instance for generating realistic test data.
    protected static readonly Faker Faker = new();

    protected readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly HttpClient HttpClient;
    private readonly KeyCloakOptions _options;
    protected readonly UsersDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Create a scoped service provider for each test.
        _scope = factory.Services.CreateScope();

        // Resolve commonly used services.
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        HttpClient = factory.CreateClient();
        _options = _scope.ServiceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
        DbContext = _scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    }

    // Authenticates against Keycloak and returns a valid JWT access token.
    protected async Task<string> GetAccessTokenAsync(string email, string password)
    {
        using var client = new HttpClient();

        // Build the authentication request expected by Keycloak.
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.PublicClientId),
            new("scope", "openid"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl))
        {
            Content = authRequestContent
        };

        // Request an access token.
        using HttpResponseMessage authorizationResponse = await client.SendAsync(authRequest);

        authorizationResponse.EnsureSuccessStatusCode();

        // Deserialize the token response.
        AuthToken authToken =
            await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>();

        return authToken!.AccessToken;
    }

    // Represents the subset of Keycloak's token response used by the tests.
    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
