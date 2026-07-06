using EventFlow.Modules.Attendance.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlow.Modules.Attendance.Infrastructure.Events;

internal sealed class EventStatisticsConfiguration : IEntityTypeConfiguration<EventStatistics>
{
    public void Configure(EntityTypeBuilder<EventStatistics> builder)
    {
        // Map the projection to the event_statistics table.
        builder.ToTable("event_statistics");

        // Use EventId as the primary key.
        builder.HasKey(es => es.EventId);

        // The identifier comes from the Event aggregate, so EF should never generate it.
        builder.Property(es => es.EventId).ValueGeneratedNever();
    }
}
