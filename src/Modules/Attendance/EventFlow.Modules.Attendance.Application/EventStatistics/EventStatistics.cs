using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventFlow.Modules.Attendance.Application.EventStatistics;

// Represents the materialized read model used for reporting.
public sealed class EventStatistics
{
    // Unique identifier of the event.
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid EventId { get; set; }

    // Basic event information.
    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime StartsAtUtc { get; set; }

    public DateTime? EndsAtUtc { get; set; }

    // Cached statistics used for fast queries.
    public int TicketsSold { get; set; }

    public int AttendeesCheckedIn { get; set; }

    // Ticket codes involved in duplicate check-in attempts.
    public List<string> DuplicateCheckInTickets { get; set; } = [];

    // Ticket codes involved in invalid check-in attempts.
    public List<string> InvalidCheckInTickets { get; set; } = [];

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
