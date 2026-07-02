using System.Reflection;
using EventFlow.ArchitectureTests.Abstractions;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Infrastructure;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Infrastructure;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.Infrastructure;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure;
using NetArchTest.Rules;

namespace EventFlow.ArchitectureTests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        // Namespaces of the modules the Users module must not reference.
        string[] otherModules = [EventsNamespace, TicketingNamespace, AttendanceNamespace];

        // Integration event namespaces are allowed dependencies.
        string[] integrationEventsModules =
        [
            EventsIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace
        ];

        // Assemblies that make up the Users module.
        List<Assembly> usersAssemblies =
        [
            typeof(User).Assembly,
            Modules.Users.Application.AssemblyReference.Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
            typeof(UsersModule).Assembly
        ];

        // Verify that the Users module only depends on integration event contracts.
        Types.InAssemblies(usersAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void EventsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        // Namespaces of the modules the Events module must not reference.
        string[] otherModules = [UsersNamespace, TicketingNamespace, AttendanceNamespace];

        // Integration event namespaces are allowed dependencies.
        string[] integrationEventsModules =
        [
            UsersIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace
        ];

        // Assemblies that make up the Events module.
        List<Assembly> eventsAssemblies =
        [
            typeof(Event).Assembly,
            Modules.Events.Application.AssemblyReference.Assembly,
            Modules.Events.Presentation.AssemblyReference.Assembly,
            typeof(EventsModule).Assembly
        ];

        // Verify that the Events module only depends on integration event contracts.
        Types.InAssemblies(eventsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void TicketingModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        // Namespaces of the modules the Ticketing module must not reference.
        string[] otherModules = [EventsNamespace, UsersNamespace, AttendanceNamespace];

        // Integration event namespaces are allowed dependencies.
        string[] integrationEventsModules =
        [
            EventsIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace
        ];

        // Assemblies that make up the Ticketing module.
        List<Assembly> ticketingAssemblies =
        [
            typeof(Order).Assembly,
            Modules.Ticketing.Application.AssemblyReference.Assembly,
            Modules.Ticketing.Presentation.AssemblyReference.Assembly,
            typeof(TicketingModule).Assembly
        ];

        // Verify that the Ticketing module only depends on integration event contracts.
        Types.InAssemblies(ticketingAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void AttendanceModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        // Namespaces of the modules the Attendance module must not reference.
        string[] otherModules = [UsersNamespace, TicketingNamespace, EventsNamespace];

        // Integration event namespaces are allowed dependencies.
        string[] integrationEventsModules =
        [
            UsersIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            EventsIntegrationEventsNamespace
        ];

        // Assemblies that make up the Attendance module.
        List<Assembly> attendanceAssemblies =
        [
            typeof(Attendee).Assembly,
            Modules.Attendance.Application.AssemblyReference.Assembly,
            Modules.Attendance.Presentation.AssemblyReference.Assembly,
            typeof(AttendanceModule).Assembly
        ];

        // Verify that the Attendance module only depends on integration event contracts.
        Types.InAssemblies(attendanceAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
