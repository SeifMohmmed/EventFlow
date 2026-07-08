using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.GetUserPermissions;
using EventFlow.Modules.Users.Application.Users.RegisterUser;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Users.IntegrationTests.Users;

// Integration tests for retrieving a user's permissions.
public class GetUserPermissionTests : BaseIntegrationTest
{
    public GetUserPermissionTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange - Use an identity ID that does not exist.
        string identityId = Guid.NewGuid().ToString();

        // Act - Request permissions for the unknown user.
        Result<PermissionsResponse> permissionsResult =
            await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert - The user should not be found.
        permissionsResult.Error.Should().Be(UserErrors.NotFound(identityId));
    }

    [Fact]
    public async Task Should_ReturnPermissions_WhenUserExists()
    {
        // Arrange - Register a new user.
        Result<Guid> result = await Sender.Send(
            new RegisterUserCommand(
                Faker.Internet.Email(),
                Faker.Internet.Password(),
                Faker.Name.FirstName(),
                Faker.Name.LastName()));

        // Retrieve the Keycloak identity ID associated with the user.
        string identityId =
            DbContext.Users.Single(u => u.Id == result.Value).IdentityId;

        // Act - Retrieve the user's permissions.
        Result<PermissionsResponse> permissionsResult =
            await Sender.Send(new GetUserPermissionsQuery(identityId));

        // Assert - The request should succeed and return permissions.
        permissionsResult.IsSuccess.Should().BeTrue();
        permissionsResult.Value.Permissions.Should().NotBeEmpty();
    }
}
