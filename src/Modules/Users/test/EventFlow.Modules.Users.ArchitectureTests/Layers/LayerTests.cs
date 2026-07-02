using EventFlow.Modules.Users.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace EventFlow.Modules.Users.ArchitectureTests.Layers;

public class LayerTests : BaseTest
{
    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        // The Domain layer must not depend on the Application layer.
        Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        // The Domain layer must not depend on the Infrastructure layer.
        Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        // The Application layer must not depend on the Infrastructure layer.
        Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        // The Application layer must not depend on the Presentation layer.
        Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void PresentationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        // The Presentation layer must not depend on the Infrastructure layer.
        Types.InAssembly(PresentationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
