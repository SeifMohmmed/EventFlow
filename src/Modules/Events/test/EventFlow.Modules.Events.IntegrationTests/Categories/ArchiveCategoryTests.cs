using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.ArchiveCategory;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Categories;

// Integration tests for archiving categories.
public class ArchiveCategoryTests : BaseIntegrationTest
{
    public ArchiveCategoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange - Use a category that does not exist.
        var command = new ArchiveCategoryCommand(Guid.NewGuid());

        // Act - Attempt to archive the category.
        Result result = await Sender.Send(command);

        // Assert - The category should not be found.
        result.Error.Should().Be(CategoryErrors.NotFound(command.CategoryId));
    }

    [Fact]
    public async Task Should_ArchiveCategory_WhenCategoryExists()
    {
        // Arrange - Create a category.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new ArchiveCategoryCommand(categoryId);

        // Act - Archive the category.
        Result result = await Sender.Send(command);

        // Assert - The category should be archived successfully.
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryAlreadyArchived()
    {
        // Arrange - Create and archive a category.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var command = new ArchiveCategoryCommand(categoryId);

        await Sender.Send(command);

        // Act - Attempt to archive it again.
        Result result = await Sender.Send(command);

        // Assert - The category is already archived.
        result.Error.Should().Be(CategoryErrors.AlreadyArchived);
    }
}
