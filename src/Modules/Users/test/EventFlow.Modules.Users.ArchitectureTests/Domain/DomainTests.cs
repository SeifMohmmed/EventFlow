using System.Reflection;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace EventFlow.Modules.Users.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        // Domain events must be sealed.
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        // Domain events must follow the "*DomainEvent" naming convention.
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        // Retrieve all domain entities.
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            // Get all non-public constructors.
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.NonPublic |
                                                                        BindingFlags.Instance);

            // Every entity should have a private parameterless constructor for EF Core.
            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        // Ensure every entity satisfies the rule.
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        // Retrieve all domain entities.
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (Type entityType in entityTypes)
        {
            // Get all public constructors.
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.Public |
                                                                        BindingFlags.Instance);

            // Entities should not expose public constructors.
            if (constructors.Any())
            {
                failingTypes.Add(entityType);
            }
        }

        // Ensure every entity satisfies the rule.
        failingTypes.Should().BeEmpty();
    }
}
