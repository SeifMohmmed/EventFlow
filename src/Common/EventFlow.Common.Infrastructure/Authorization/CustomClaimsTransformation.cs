using System.Security.Claims;
using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation(
    IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Skip transformation if the custom claims have already been added.
        if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
        {
            return principal;
        }

        // Create a new DI scope for resolving scoped services.
        using IServiceScope scope = serviceScopeFactory.CreateScope();

        // Resolve the permission service.
        IPermissionService permissionService =
            scope.ServiceProvider.GetRequiredService<IPermissionService>();

        // Get the user's identity provider ID from the JWT.
        string identityId = principal.GetIdentityId();

        // Retrieve the application's user ID and permissions.
        Result<PermissionsResponse> result =
            await permissionService.GetUserPermissionsAsync(identityId);

        // Stop authentication if the permissions couldn't be retrieved.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(IPermissionService.GetUserPermissionsAsync),
                result.Error);
        }

        // Create a new identity that will hold the custom claims.
        var claimsIdentity = new ClaimsIdentity();

        // Add the application's user ID as the "sub" claim.
        claimsIdentity.AddClaim(
            new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));

        // Add every permission as an individual claim.
        foreach (string permission in result.Value.Permissions)
        {
            claimsIdentity.AddClaim(
                new Claim(CustomClaims.Permission, permission));
        }

        // Attach the new identity to the authenticated user.
        principal.AddIdentity(claimsIdentity);

        // Return the enriched principal.
        return principal;
    }
}
