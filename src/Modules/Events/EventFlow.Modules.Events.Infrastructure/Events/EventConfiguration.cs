using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Modules.Events.Infrastructure.Events;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasOne<Category>().WithMany();
    }
}
