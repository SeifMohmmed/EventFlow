using EventFlow.Modules.Api.Events;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Api.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
{
    internal DbSet<Event> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);
    }
}
