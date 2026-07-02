using EventFlow.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace EventFlow.Common.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Get all permissions assigned to the current user.
        HashSet<string> permissions = context.User.GetPermissions();

        // Succeed if the user has the required permission.
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        // No asynchronous work is required.
        return Task.CompletedTask;
    }
}
