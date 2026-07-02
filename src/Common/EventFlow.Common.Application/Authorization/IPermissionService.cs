using EventFlow.Common.Domain;

namespace EventFlow.Common.Application.Authorization;

public interface IPermissionService
{
    // Retrieves all permissions for the specified identity provider user.
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
