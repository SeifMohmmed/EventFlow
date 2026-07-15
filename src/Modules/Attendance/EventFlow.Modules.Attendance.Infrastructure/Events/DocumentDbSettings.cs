namespace EventFlow.Modules.Attendance.Infrastructure.Events;

// Defines MongoDB database and collection names used by the Attendance module.
internal static class DocumentDbSettings
{
    // MongoDB database name.
    internal const string Database = "attendance";

    // Collection storing the Event Statistics projection.
    internal const string EventStatistics = "event-statistics";
}
