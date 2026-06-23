using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);
    }
}
