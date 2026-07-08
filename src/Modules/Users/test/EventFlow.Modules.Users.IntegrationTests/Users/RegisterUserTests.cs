using System.Net;
using System.Net.Http.Json;
using EventFlow.Modules.Users.IntegrationTests.Abstractions;
using EventFlow.Modules.Users.Presentation.Users;
using FluentAssertions;

namespace EventFlow.Modules.Users.IntegrationTests.Users;

// Integration tests for user registration.
public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    // Invalid registration requests used to verify validation rules.
    public static readonly TheoryData<string, string, string, string> InvalidRequests = new()
    {
        { "", Faker.Internet.Password(), Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), "12345", Faker.Name.FirstName(), Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), "", Faker.Name.LastName() },
        { Faker.Internet.Email(), Faker.Internet.Password(), Faker.Name.FirstName(), "" }
    };

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsNotValid(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        // Arrange - Build an invalid registration request.
        var request = new RegisterUser.Request
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName
        };

        // Act - Submit the registration request.
        HttpResponseMessage response =
            await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert - Validation should fail.
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange - Build a valid registration request.
        var request = new RegisterUser.Request
        {
            Email = "create@test.com",
            Password = Faker.Internet.Password(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName()
        };

        // Act - Register the user.
        HttpResponseMessage response =
            await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert - Registration should succeed.
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnAccessToken_WhenUserIsRegistered()
    {
        // Arrange - Register a new user.
        var request = new RegisterUser.Request
        {
            Email = "token@test.com",
            Password = Faker.Internet.Password(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName()
        };

        await HttpClient.PostAsJsonAsync("users/register", request);

        // Act - Authenticate using the registered credentials.
        string accessToken =
            await GetAccessTokenAsync(request.Email, request.Password);

        // Assert - A valid JWT access token should be returned.
        accessToken.Should().NotBeEmpty();
    }
}
