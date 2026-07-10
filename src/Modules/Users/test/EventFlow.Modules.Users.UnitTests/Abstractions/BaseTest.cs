using Bogus;
using EventFlow.Common.Domain;

namespace EventFlow.Modules.Users.UnitTests.Abstractions;

#pragma warning disable CA1515

// Base class shared by all Users module unit tests.
public abstract class BaseTest
{
    protected static readonly Faker Faker = new();

    // Verifies that the specified domain event was published by the entity.
    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        T? domainEvent = entity.DomainEvents.OfType<T>().SingleOrDefault();

        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} was not published");
        }

        return domainEvent;
    }
}
