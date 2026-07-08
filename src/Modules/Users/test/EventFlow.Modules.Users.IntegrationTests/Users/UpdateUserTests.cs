using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.RegisterUser;
using EventFlow.Modules.Users.Application.Users.UpdateUser;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Users.IntegrationTests.Users;

// Integration tests for updating users.
public class UpdateUserTests : BaseIntegrationTest
{
    public UpdateUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    // Invalid commands used to verify validation rules.
    public static readonly TheoryData<UpdateUserCommand> InvalidCommands = new()
    {
        new UpdateUserCommand(Guid.Empty, Faker.Name.FirstName(), Faker.Name.LastName()),
        new UpdateUserCommand(Guid.NewGuid(), "", Faker.Name.LastName()),
        new UpdateUserCommand(Guid.NewGuid(), Faker.Name.FirstName(), "")
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnError_WhenCommandIsNotValid(UpdateUserCommand command)
    {
        // Act - Execute the invalid command.
        Result result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange - Use a user ID that does not exist.
        var userId = Guid.NewGuid();

        // Act - Attempt to update the unknown user.
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(
                userId,
                Faker.Name.FirstName(),
                Faker.Name.LastName()));

        // Assert - The user should not be found.
        updateResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUserExists()
    {
        // Arrange - Register a new user.
        Result<Guid> result = await Sender.Send(
            new RegisterUserCommand(
                Faker.Internet.Email(),
                Faker.Internet.Password(),
                Faker.Name.FirstName(),
                Faker.Name.LastName()));

        Guid userId = result.Value;

        // Act - Update the user's profile information.
        Result updateResult = await Sender.Send(
            new UpdateUserCommand(
                userId,
                Faker.Name.FirstName(),
                Faker.Name.LastName()));

        // Assert - The update should complete successfully.
        updateResult.IsSuccess.Should().BeTrue();
    }
}
