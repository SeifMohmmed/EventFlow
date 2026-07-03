using System.Reflection;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.Infrastructure;

namespace EventFlow.Modules.Ticketing.ArchitectureTests.Abstractions;
#pragma warning disable CA1515
public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Ticketing.Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(Order).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(TicketingModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Ticketing.Presentation.AssemblyReference).Assembly;
}
