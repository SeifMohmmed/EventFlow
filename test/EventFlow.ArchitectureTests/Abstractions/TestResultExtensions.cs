using FluentAssertions;
using NetArchTest.Rules;

namespace EventFlow.ArchitectureTests;

internal static class TestResultExtensions
{
    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        // Verify that no types violated the architecture rule.
        testResult.FailingTypes?.Should().BeEmpty();
    }
}

