using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Infrastructure.Authentication;
using EventFlow.Modules.Attendance.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace EventFlow.Modules.Attendance.Infrastructure.Authentication;

internal sealed class AttendanceContext(IHttpContextAccessor httpContextAccessor) : IAttendanceContext
{
    public Guid AttendeeId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EventFlowException("User identifier is unavailable");
}
