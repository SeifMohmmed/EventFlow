using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        // Map the entity to the outbox_messages table.
        builder.ToTable("outbox_messages");

        // Use the event ID as the primary key.
        builder.HasKey(o => o.Id);

        // Store the serialized event as PostgreSQL JSONB.
        builder.Property(o => o.Content)
            .HasMaxLength(2000)
            .HasColumnType("jsonb");
    }
}
