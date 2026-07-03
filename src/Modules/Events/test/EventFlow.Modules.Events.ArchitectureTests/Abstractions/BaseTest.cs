using System.Reflection;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Infrastructure;

namespace EventFlow.Modules.Events.ArchitectureTests.Abstractions;

#pragma warning disable CA1515
public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Events.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Event).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(EventsModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Events.Presentation.AssemblyReference).Assembly;
}
