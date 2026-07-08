using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using EventFlow.Modules.Users.Application.Users.GetUser;
using EventFlow.Modules.Users.IntegrationTests.Abstractions;
using EventFlow.Modules.Users.Presentation.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EventFlow.Modules.Users.IntegrationTests.Users;

// Integration tests for the authenticated user profile endpoint.
public class GetUserProfileTests : BaseIntegrationTest
{
    public GetUserProfileTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnUnauthorized_WhenAccessTokenNotProvided()
    {
        // Act - Call the endpoint without authentication.
        HttpResponseMessage response =
            await HttpClient.GetAsync("users/profile");

        // Assert - Access should be denied.
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUserExists()
    {
        // Arrange - Register a user and authenticate.
        string accessToken =
            await RegisterUserAndGetAccessTokenAsync(
                "exists@test.com",
                Faker.Internet.Password());

        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                accessToken);

        // Act - Request the authenticated user's profile.
        HttpResponseMessage response =
            await HttpClient.GetAsync("users/profile");

        // Assert - The profile should be returned successfully.
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        UserResponse? user =
            await response.Content.ReadFromJsonAsync<UserResponse>();

        user.Should().NotBeNull();
    }

    // Registers a user and returns a valid JWT access token.
    private async Task<string> RegisterUserAndGetAccessTokenAsync(
        string email,
        string password)
    {
        var request = new RegisterUser.Request
        {
            Email = email,
            Password = password,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName()
        };

        // Register the user through the API.
        await HttpClient.PostAsJsonAsync("users/register", request);

        // Authenticate the newly created user.
        string accessToken =
            await GetAccessTokenAsync(request.Email, request.Password);

        return accessToken;
    }
}
