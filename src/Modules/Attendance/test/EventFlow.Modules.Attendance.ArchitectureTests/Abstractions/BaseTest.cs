using System.Reflection;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Infrastructure;

namespace EventFlow.Modules.Attendance.ArchitectureTests.Abstractions;
#pragma warning disable CA1515
public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Attendance.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Attendee).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(AttendanceModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Attendance.Presentation.AssemblyReference).Assembly;
}
