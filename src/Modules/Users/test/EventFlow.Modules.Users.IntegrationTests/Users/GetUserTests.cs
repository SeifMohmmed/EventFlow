using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.GetUser;
using EventFlow.Modules.Users.Application.Users.RegisterUser;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Users.IntegrationTests.Users;

// Integration tests for retrieving users.
public class GetUserTests : BaseIntegrationTest
{
    public GetUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange - Use a user ID that does not exist.
        var userId = Guid.NewGuid();

        // Act - Attempt to retrieve the user.
        Result<UserResponse> userResult =
            await Sender.Send(new GetUserQuery(userId));

        // Assert - The user should not be found.
        userResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange - Register a new user.
        Result<Guid> result = await Sender.Send(
            new RegisterUserCommand(
                Faker.Internet.Email(),
                Faker.Internet.Password(),
                Faker.Name.FirstName(),
                Faker.Name.LastName()));

        Guid userId = result.Value;

        // Act - Retrieve the registered user.
        Result<UserResponse> userResult =
            await Sender.Send(new GetUserQuery(userId));

        // Assert - The request should succeed and return the user.
        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();
    }
}
