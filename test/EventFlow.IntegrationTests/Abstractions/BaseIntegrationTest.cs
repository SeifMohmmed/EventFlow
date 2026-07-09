using Bogus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Base class for integration tests providing shared services and test data generation.
[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    private readonly IServiceScope _scope;

    protected readonly ISender Sender;

    // Shared Faker instance for generating realistic test data.
    protected readonly Faker Faker = new();

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Create a scoped service provider for the test.
        _scope = factory.Services.CreateScope();

        // Resolve MediatR for executing commands and queries.
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
