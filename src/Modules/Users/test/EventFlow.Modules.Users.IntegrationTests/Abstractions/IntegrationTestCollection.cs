namespace EventFlow.Modules.Users.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Shares a single IntegrationTestWebAppFactory instance across the test collection.
[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection
    : ICollectionFixture<IntegrationTestWebAppFactory>;
