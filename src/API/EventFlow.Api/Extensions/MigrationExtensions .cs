using EventFlow.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Api.Extensions;

/// <summary>
/// Provides extension methods for applying
/// database migrations at application startup.
/// </summary>
internal static class MigrationExtensions
{
    /// <summary>
    /// Applies pending migrations for all registered module DbContexts.
    /// </summary>
    internal static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ApplyMigration<EventsDbContext>(scope);
    }

    /// <summary>
    /// Applies pending migrations for the specified DbContext.
    /// </summary>
    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using TDbContext context =
            scope.ServiceProvider.GetRequiredService<TDbContext>();

        context.Database.Migrate();
    }
}
