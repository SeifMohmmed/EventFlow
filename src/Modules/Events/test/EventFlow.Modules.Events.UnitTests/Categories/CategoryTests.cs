using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Events.UnitTests.Categories;

// Unit tests for the Category aggregate.
public class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
    {
        // Act - Create a new category.
        Result<Category> result = Category.Create(Faker.Music.Genre());

        // Assert - Verify the CategoryCreated domain event was published.
        CategoryCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);

        domainEvent.CategoryId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
    {
        // Arrange - Create a category to archive.
        Result<Category> result = Category.Create(Faker.Music.Genre());

        Category category = result.Value;

        // Act - Archive the category.
        category.Archive();

        // Assert - Verify the CategoryArchived domain event was published.
        CategoryArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
    {
        // Arrange - Create a category and clear previous events.
        Result<Category> result = Category.Create(Faker.Music.Genre());

        Category category = result.Value;
        category.ClearDomainEvents();

        string newName = Faker.Music.Genre();

        // Act - Change the category name.
        category.ChangeName(newName);

        // Assert - Verify the CategoryNameChanged domain event was published.
        CategoryNameChangedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }
}
