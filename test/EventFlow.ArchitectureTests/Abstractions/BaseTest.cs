namespace EventFlow.ArchitectureTests.Abstractions;

#pragma warning disable CA1515
public abstract class BaseTest
{
    // Namespace of the Users module.
    protected const string UsersNamespace = "EventFlow.Modules.Users";

    // Namespace containing the Users integration events.
    protected const string UsersIntegrationEventsNamespace = "EventFlow.Modules.Users.IntegrationEvents";

    // Namespace of the Events module.
    protected const string EventsNamespace = "EventFlow.Modules.Events";

    // Namespace containing the Events integration events.
    protected const string EventsIntegrationEventsNamespace = "EventFlow.Modules.Events.IntegrationEvents";

    // Namespace of the Ticketing module.
    protected const string TicketingNamespace = "EventFlow.Modules.Ticketing";

    // Namespace containing the Ticketing integration events.
    protected const string TicketingIntegrationEventsNamespace = "EventFlow.Modules.Ticketing.IntegrationEvents";

    // Namespace of the Attendance module.
    protected const string AttendanceNamespace = "EventFlow.Modules.Attendance";

    // Namespace containing the Attendance integration events.
    protected const string AttendanceIntegrationEventsNamespace = "EventFlow.Modules.Attendance.IntegrationEvents";
}
