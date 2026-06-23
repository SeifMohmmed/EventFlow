using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.Infrastructure.Database;

namespace EventFlow.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
