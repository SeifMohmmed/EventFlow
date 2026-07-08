using Bogus;
using EventFlow.Common.Domain;

namespace EventFlow.Modules.Events.UnitTests.Abstractions;

#pragma warning disable CA1515

// Base class for unit tests providing shared test utilities.
public abstract class BaseTest
{
    // Shared Faker instance for generating realistic test data.
    protected static readonly Faker Faker = new();

    // Verifies that the specified domain event was published by the entity.
    public static T AssertDomainEventWasPublished<T>(Entity entity)
    {
        // Find the first published domain event of the requested type.
        T? domainEvent = entity.DomainEvents.OfType<T>().FirstOrDefault();

        // Fail the test if the expected event was not raised.
        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} was not published.");
        }

        return domainEvent;
    }
}
