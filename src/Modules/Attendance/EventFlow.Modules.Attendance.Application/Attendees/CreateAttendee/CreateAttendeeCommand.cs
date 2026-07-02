using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Attendance.Application.Attendees.CreateAttendee;

public sealed record CreateAttendeeCommand(Guid AttendeeId, string Email, string FirstName, string LastName)
    : ICommand;
