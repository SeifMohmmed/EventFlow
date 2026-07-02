using Microsoft.AspNetCore.Authorization;

namespace EventFlow.Common.Infrastructure.Authorization;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    // Creates a permission requirement for the specified permission.
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    // The permission required to access the resource.
    public string Permission { get; }
}
