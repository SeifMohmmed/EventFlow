using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Events;
using EventFlow.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public async Task<Event?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
