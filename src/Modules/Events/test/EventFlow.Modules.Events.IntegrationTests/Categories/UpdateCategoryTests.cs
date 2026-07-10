using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.UpdateCategory;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Categories;

// Integration tests for updating categories.
public class UpdateCategoryTests : BaseIntegrationTest
{
    public UpdateCategoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    // Invalid commands used to verify validation rules.
    public static readonly TheoryData<UpdateCategoryCommand> InvalidCommands = new()
    {
        new UpdateCategoryCommand(Guid.Empty, Faker.Music.Genre()),
        new UpdateCategoryCommand(Guid.NewGuid(), string.Empty)
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnFailure_WhenCommandIsNotValid(UpdateCategoryCommand command)
    {
        // Act - Execute the invalid command.
        Result result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange - Use a category that does not exist.
        var command = new UpdateCategoryCommand(
            Guid.NewGuid(),
            Faker.Music.Genre());

        // Act - Attempt to update the category.
        Result result = await Sender.Send(command);

        // Assert - The category should not be found.
        result.Error.Should().Be(CategoryErrors.NotFound(command.CategoryId));
    }

    [Fact]
    public async Task Should_UpdateCategory_WhenCategoryExists()
    {
        // Arrange - Create a category.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new UpdateCategoryCommand(
            categoryId,
            Faker.Music.Genre());

        // Act - Update the category.
        Result result = await Sender.Send(command);

        // Assert - The category should be updated successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
