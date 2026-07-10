using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.CreateCategory;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Categories;

// Integration tests for category creation.
public class CreateCategoryTests : BaseIntegrationTest
{
    public CreateCategoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_CreateCategory_WhenCommandIsValid()
    {
        // Arrange - Build a valid category creation command.
        var command = new CreateCategoryCommand("Category name");

        // Act - Create the category.
        Result<Guid> result = await Sender.Send(command);

        // Assert - The category should be created successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsNotValid()
    {
        // Arrange - Build an invalid category creation command.
        var command = new CreateCategoryCommand("");

        // Act - Execute the command.
        Result<Guid> result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
    }
}
