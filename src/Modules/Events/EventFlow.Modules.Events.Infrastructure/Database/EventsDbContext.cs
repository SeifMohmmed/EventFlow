using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Categories;
using EventFlow.Modules.Events.Domain.TicketTypes;
using EventFlow.Modules.Events.Infrastructure.Events;
using EventFlow.Modules.Events.Infrastructure.TicketTypes;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events { get; set; }
    internal DbSet<Category> Categories { get; set; }

    internal DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);

        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new TicketTypeConfiguration());
    }
}
