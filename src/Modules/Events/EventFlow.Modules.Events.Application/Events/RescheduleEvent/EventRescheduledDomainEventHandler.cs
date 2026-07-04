using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Events.Domain.Events;

namespace EventFlow.Modules.Events.Application.Events.RescheduleEvent;

/// <summary>
/// Handles the <see cref="EventRescheduledDomainEvent"/>.
/// </summary>
internal sealed class EventRescheduledDomainEventHandler
    : DomainEventHandler<EventRescheduledDomainEvent>
{
    /// <summary>
    /// Executes the logic that should run after an event
    /// has been rescheduled.
    /// </summary>
    public override Task Handle(
        EventRescheduledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // No additional processing is required at this time.
        return Task.CompletedTask;
    }
}
