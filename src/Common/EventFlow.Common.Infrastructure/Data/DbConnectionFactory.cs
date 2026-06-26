using System.Data.Common;
using EventFlow.Common.Application.Data;
using Npgsql;

namespace EventFlow.Common.Infrastructure.Data;

/// <summary>
/// Creates and opens database connections using <see cref="NpgsqlDataSource"/>.
/// </summary>
internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    /// <summary>
    /// Opens a new PostgreSQL database connection.
    /// </summary>
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
