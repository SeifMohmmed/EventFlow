using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Attendance.Application.Attendees.UpdateAttendee;

public sealed record UpdateAttendeeCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;
