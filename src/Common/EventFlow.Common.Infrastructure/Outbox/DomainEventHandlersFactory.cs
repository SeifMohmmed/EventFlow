using System.Collections.Concurrent;
using System.Reflection;
using EventFlow.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    // Cache handler types to avoid repeated reflection.
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventHandler> GetHandlers(
        Type type,
        IServiceProvider serviceProvider,
        Assembly assembly)
    {
        // Get the handler types from the cache or discover them using reflection.
        Type[] domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}{type.Name}",
            _ =>
            {
                Type[] domainEventHandlerTypes = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(
                        typeof(IDomainEventHandler<>).MakeGenericType(type)))
                    .ToArray();

                return domainEventHandlerTypes;
            });

        var handlers = new List<IDomainEventHandler>();

        foreach (Type domainEventHandlerType in domainEventHandlerTypes)
        {
            // Resolve the handler from the dependency injection container.
            object domainEventHandler =
                serviceProvider.GetRequiredService(domainEventHandlerType);

            // Add the resolved handler to the result collection.
            handlers.Add((IDomainEventHandler)domainEventHandler);
        }

        return handlers;
    }
}
