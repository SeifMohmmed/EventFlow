namespace EventFlow.Modules.Attendance.Domain.Events;

// Represents the materialized read model used for reporting.
public sealed class EventStatistics
{
    public Guid EventId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime? EndsAtUtc { get; private set; }

    // Cached statistics used for fast queries.
    public int TicketsSold { get; private set; }

    public int AttendeesCheckedIn { get; private set; }

    public List<string> DuplicateCheckInTickets { get; private set; }

    public List<string> InvalidCheckInTickets { get; private set; }

    // Factory method for creating the initial projection.
    public static EventStatistics Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        var @event = new EventStatistics
        {
            EventId = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc
        };

        return @event;
    }
}
