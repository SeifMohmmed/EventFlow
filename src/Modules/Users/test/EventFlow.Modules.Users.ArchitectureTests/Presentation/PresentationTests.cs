using EventFlow.Modules.Users.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;

namespace EventFlow.Modules.Users.ArchitectureTests.Presentation;

public class PresentationTests : BaseTest
{
    [Fact]
    public void IntegrationEventHandler_Should_BeSealed()
    {
        // Integration event consumers must be sealed.
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventHandler_ShouldHave_NameEndingWith_DomainEventHandler()
    {
        // Integration event consumers must follow the "*Consumer" naming convention.
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("Consumer")
            .GetResult()
            .ShouldBeSuccessful();
    }
}
