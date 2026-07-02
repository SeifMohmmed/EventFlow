using System.Reflection;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.Infrastructure;

namespace EventFlow.Modules.Users.ArchitectureTests.Abstractions;

#pragma warning disable CA1515
public abstract class BaseTest
{
    // Reference to the Application assembly.
    protected static readonly Assembly ApplicationAssembly =
        typeof(Users.Application.AssemblyReference).Assembly;

    // Reference to the Domain assembly.
    protected static readonly Assembly DomainAssembly =
        typeof(User).Assembly;

    // Reference to the Infrastructure assembly.
    protected static readonly Assembly InfrastructureAssembly =
        typeof(UsersModule).Assembly;

    // Reference to the Presentation assembly.
    protected static readonly Assembly PresentationAssembly =
        typeof(Users.Presentation.AssemblyReference).Assembly;

}
