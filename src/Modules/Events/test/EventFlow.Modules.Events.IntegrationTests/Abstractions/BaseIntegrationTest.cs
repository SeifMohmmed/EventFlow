using Bogus;
using EventFlow.Modules.Events.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Events.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
#pragma warning disable CA1515

// Base class for Events integration tests.
public abstract class BaseIntegrationTest : IDisposable
{
    // Shared Faker instance for generating realistic test data.
    protected static readonly Faker Faker = new();

    private readonly IServiceScope _scope;

    // Test application factory.
    protected readonly IntegrationTestWebAppFactory Factory;

    // MediatR entry point used to execute commands and queries.
    protected readonly ISender Sender;

    // Database context used for assertions and cleanup.
    protected readonly EventsDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Create a scoped service provider for the test.
        _scope = factory.Services.CreateScope();

        // Resolve commonly used services.
        Factory = factory;
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<EventsDbContext>();
    }

    // Removes all persisted test data to keep tests isolated.
    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM events.inbox_message_consumers;
            DELETE FROM events.inbox_messages;
            DELETE FROM events.outbox_message_consumers;
            DELETE FROM events.outbox_messages;
            DELETE FROM events.ticket_types;
            DELETE FROM events.events;
            DELETE FROM events.categories;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
