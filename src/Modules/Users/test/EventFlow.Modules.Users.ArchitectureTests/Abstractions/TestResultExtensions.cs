using FluentAssertions;
using NetArchTest.Rules;

namespace EventFlow.Modules.Users.ArchitectureTests.Abstractions;

internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        // Verify that no types violated the architecture rule.
        testResult.FailingTypes?.Should().BeEmpty();
    }
}
