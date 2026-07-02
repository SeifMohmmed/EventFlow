using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace EventFlow.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        // Send a query to retrieve the user's permissions.
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
