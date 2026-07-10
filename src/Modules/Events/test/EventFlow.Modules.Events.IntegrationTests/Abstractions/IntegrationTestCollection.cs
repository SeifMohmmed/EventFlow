namespace EventFlow.Modules.Events.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Shares a single IntegrationTestWebAppFactory instance across all integration tests.
[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection
    : ICollectionFixture<IntegrationTestWebAppFactory>;
