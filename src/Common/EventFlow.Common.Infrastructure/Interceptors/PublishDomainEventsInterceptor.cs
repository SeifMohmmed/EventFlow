using EventFlow.Common.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Infrastructure.Interceptors;

/// <summary>
/// EF Core interceptor that publishes domain events
/// after a successful database transaction.
/// </summary>
public sealed class PublishDomainEventsInterceptor(
    IServiceScopeFactory serviceScopeFactory)
    : SaveChangesInterceptor
{
    /// <summary>
    /// Publishes all domain events raised during the current
    /// SaveChanges operation.
    /// </summary>
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        // Publish domain events only when a DbContext is available.
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context);
        }

        return await base.SavedChangesAsync(
            eventData,
            result,
            cancellationToken);
    }

    /// <summary>
    /// Collects and publishes all domain events raised
    /// by tracked entities.
    /// </summary>
    private async Task PublishDomainEventsAsync(DbContext context)
    {
        // Collect domain events from all tracked entities and
        // clear them to prevent duplicate publishing.
        var domainEvents = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents =
                    entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        // Resolve a publisher from a new DI scope.
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPublisher publisher =
            scope.ServiceProvider.GetRequiredService<IPublisher>();

        // Publish each domain event.
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
