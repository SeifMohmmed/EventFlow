using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.Categories.GetCategories;
using EventFlow.Modules.Events.Application.Categories.GetCategory;
using EventFlow.Modules.Events.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.IntegrationTests.Categories;

// Integration tests for retrieving categories.
public class GetCategoriesTests : BaseIntegrationTest
{
    public GetCategoriesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnEmptyCollection_WhenNoCategoriesExist()
    {
        // Arrange - Ensure the database contains no categories.
        await CleanDatabaseAsync();

        var query = new GetCategoriesQuery();

        // Act - Retrieve all categories.
        Result<IReadOnlyCollection<CategoryResponse>> result =
            await Sender.Send(query);

        // Assert - No categories should be returned.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange - Create two categories.
        await CleanDatabaseAsync();

        await Sender.CreateCategoryAsync(Faker.Music.Genre());
        await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var query = new GetCategoriesQuery();

        // Act - Retrieve all categories.
        Result<IReadOnlyCollection<CategoryResponse>> result =
            await Sender.Send(query);

        // Assert - Both categories should be returned.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}
