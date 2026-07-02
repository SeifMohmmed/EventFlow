using System.Security.Claims;
using EventFlow.Common.Application.Exceptions;

namespace EventFlow.Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        // Read the application's user ID from the "sub" claim.
        string? userId = principal?.FindFirst(CustomClaims.Sub)?.Value;

        // Parse the claim value or throw if it is missing or invalid.
        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new EventFlowException("User identifier is unavailable");
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        // Return the identity provider user ID.
        return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? throw new EventFlowException("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        // Retrieve all permission claims from the current user.
        IEnumerable<Claim> permissionClaims =
            principal?.FindAll(CustomClaims.Permission)
            ?? throw new EventFlowException("Permissions are unavailable");

        // Return the permission values as a unique set.
        return permissionClaims
            .Select(c => c.Value)
            .ToHashSet();
    }
}
