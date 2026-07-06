namespace EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

// DTO returned to API consumers containing the precomputed event statistics.
public sealed record EventStatisticsResponse(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    int TicketsSold,
    int AttendeesCheckedIn,
    string[] DuplicateCheckInTickets,
    string[] InvalidCheckInTickets);
