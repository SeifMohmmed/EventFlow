using System.Data.Common;
using EventFlow.Modules.Events.Application.Abstractions.Data;
using Npgsql;

namespace EventFlow.Modules.Events.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
