using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Attendance.Application.Attendees.CheckInAttendee;

public sealed record CheckInAttendeeCommand(Guid AttendeeId, Guid TicketId) : ICommand;
