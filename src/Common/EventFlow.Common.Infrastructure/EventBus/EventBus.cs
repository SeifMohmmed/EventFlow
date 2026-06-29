using EventFlow.Common.Application.EventBus;
using MassTransit;

namespace EventFlow.Common.Infrastructure.EventBus;

/// <summary>
/// Publishes integration events using MassTransit.
/// </summary>
internal sealed class EventBus(IBus bus) : IEventBus
{
    /// <summary>
    /// Publishes the specified integration event.
    /// </summary>
    public async Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
