using Bogus;
using EventFlow.Modules.Attendance.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Attendance.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
#pragma warning disable CA1515

// Base class for Attendance integration tests.
public abstract class BaseIntegrationTest : IDisposable
{
    // Shared Faker instance for generating realistic test data.
    protected static readonly Faker Faker = new();

    private readonly IServiceScope _scope;

    protected readonly ISender Sender;

    protected readonly AttendanceDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        // Create a scoped service provider for each test.
        _scope = factory.Services.CreateScope();

        // Resolve commonly used services.
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<AttendanceDbContext>();
    }

    // Removes all test data to ensure database isolation between tests.
    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM attendance.inbox_message_consumers;
            DELETE FROM attendance.inbox_messages;
            DELETE FROM attendance.outbox_message_consumers;
            DELETE FROM attendance.outbox_messages;
            DELETE FROM attendance.attendees;
            DELETE FROM attendance.events;
            DELETE FROM attendance.tickets;
            DELETE FROM attendance.event_statistics;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
