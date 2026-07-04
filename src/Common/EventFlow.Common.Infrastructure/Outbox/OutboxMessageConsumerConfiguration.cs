using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumerConfiguration
    : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        // Map the entity to the outbox_message_consumers table.
        builder.ToTable("outbox_message_consumers");

        // Use the message ID and consumer name as a composite primary key.
        builder.HasKey(o => new { o.OutboxMessageId, o.Name });

        // Limit the maximum length of the consumer name.
        builder.Property(o => o.Name)
            .HasMaxLength(500);
    }
}
