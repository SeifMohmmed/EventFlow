using System.Data.Common;
using Dapper;
using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Application.Data;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Domain.Users;

namespace EventFlow.Modules.Users.Application.Users.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(
        GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        // Open a database connection.
        await using DbConnection connection =
            await dbConnectionFactory.OpenConnectionAsync();

        // Query all permissions assigned to the user through their roles.
        const string sql =
            $"""
             SELECT DISTINCT
                 u.id AS {nameof(UserPermission.UserId)},
                 rp.permission_code AS {nameof(UserPermission.Permission)}
             FROM users.users u
             JOIN users.user_roles ur ON ur.user_id = u.id
             JOIN users.role_permissions rp ON rp.role_name = ur.role_name
             WHERE u.identity_id = @IdentityId
             """;

        // Execute the query and map the results.
        List<UserPermission> permissions =
            (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        // Return a failure if no user or permissions were found.
        if (!permissions.Any())
        {
            return Result.Failure<PermissionsResponse>(
                UserErrors.NotFound(request.IdentityId));
        }

        // Return the user ID and the unique set of permissions.
        return new PermissionsResponse(
            permissions[0].UserId,
            permissions.Select(p => p.Permission).ToHashSet());
    }

    /// <summary>
    /// Represents a permission record returned from the database.
    /// </summary>
    internal sealed class UserPermission
    {
        // The application's user ID.
        internal Guid UserId { get; init; }

        // The permission assigned to the user.
        internal string Permission { get; init; }
    }
}
