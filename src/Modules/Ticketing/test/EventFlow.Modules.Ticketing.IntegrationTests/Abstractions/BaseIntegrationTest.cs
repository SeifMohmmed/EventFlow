using Bogus;
using EventFlow.Modules.Ticketing.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;

#pragma warning disable CA1515

// Base class for Ticketing integration tests.
[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    // Shared Faker instance for generating realistic test data.
    protected static readonly Faker Faker = new();

    private readonly IServiceScope _scope;

    // MediatR entry point used to execute commands and queries.
    protected readonly ISender Sender;

    // Database context used for assertions and cleanup.
    protected readonly TicketingDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Create a scoped service provider for the test.
        _scope = factory.Services.CreateScope();

        // Resolve commonly used services.
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<TicketingDbContext>();
    }

    // Removes all persisted test data to keep tests isolated.
    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM ticketing.inbox_message_consumers;
            DELETE FROM ticketing.inbox_messages;
            DELETE FROM ticketing.outbox_message_consumers;
            DELETE FROM ticketing.outbox_messages;
            DELETE FROM ticketing.events;
            DELETE FROM ticketing.ticket_types;
            DELETE FROM ticketing.customers;
            DELETE FROM ticketing.orders;
            DELETE FROM ticketing.order_items;
            DELETE FROM ticketing.tickets;
            DELETE FROM ticketing.payments;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
