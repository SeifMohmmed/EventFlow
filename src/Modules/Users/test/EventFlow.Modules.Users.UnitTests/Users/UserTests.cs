using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Users.UnitTests.Users;

// Unit tests for the User aggregate.
public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnUser()
    {
        // Act - Create a new user.
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        // Assert - Verify the user was created.
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldReturnUser_WithMemberRole()
    {
        // Act - Create a new user.
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        // Assert - New users should receive the Member role by default.
        user.Roles.Single().Should().Be(Role.Member);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        // Act - Create a new user.
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        // Assert - Verify the user registered domain event was published.
        UserRegisteredDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        // Arrange - Create a user.
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        // Act - Update the user's profile.
        user.Update(user.LastName, user.FirstName);

        // Assert - Verify the profile updated domain event was published.
        UserProfileUpdatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserProfileUpdatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.FirstName.Should().Be(user.FirstName);
        domainEvent.LastName.Should().Be(user.LastName);
    }

    [Fact]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        // Arrange - Create a user and clear the registration event.
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        user.ClearDomainEvents();

        // Act - Update the user with the same values.
        user.Update(user.FirstName, user.LastName);

        // Assert - No new domain event should be published.
        user.DomainEvents.Should().BeEmpty();
    }
}
