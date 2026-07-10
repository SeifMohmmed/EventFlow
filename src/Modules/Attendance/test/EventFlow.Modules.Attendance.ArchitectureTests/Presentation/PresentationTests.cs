using EventFlow.Common.Application.EventBus;
using EventFlow.Modules.Attendance.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;

namespace EventFlow.Modules.Attendance.ArchitectureTests.Presentation;

public class PresentationTests : BaseTest
{
    [Fact]
    public void IntegrationEventHandler_Should_NotBePublic()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            // Find all integration event handlers in the Presentation assembly.
            .ImplementInterface(typeof(IIntegrationEventHandler<>))
            .Or()
            .Inherit(typeof(IntegrationEventHandler<>))
            .Should()
            // Integration event handlers are internal implementation details.
            .NotBePublic()
            .GetResult()
            .ShouldBeSuccessful();
    }
    [Fact]
    public void IntegrationEventConsumer_Should_BeSealed()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventConsumer_ShouldHave_NameEndingWith_IntegrationEventConsumer()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventConsumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
