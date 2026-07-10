using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.GetCategory;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Categories;

// Integration tests for retrieving a single category.
public class GetCategoryTests : BaseIntegrationTest
{
    public GetCategoryTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange - Use a category that does not exist.
        var query = new GetCategoryQuery(Guid.NewGuid());

        // Act - Attempt to retrieve the category.
        Result result = await Sender.Send(query);

        // Assert - The category should not be found.
        result.Error.Should().Be(CategoryErrors.NotFound(query.CategoryId));
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange - Create a category.
        Guid categoryId = await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var query = new GetCategoryQuery(categoryId);

        // Act - Retrieve the category.
        Result<CategoryResponse> result = await Sender.Send(query);

        // Assert - The category should be returned successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
