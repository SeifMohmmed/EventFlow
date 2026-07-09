using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Attendance.Application.Attendees.GetAttendee;

public sealed record GetAttendeeQuery(Guid CustomerId) : IQuery<AttendeeResponse>;
