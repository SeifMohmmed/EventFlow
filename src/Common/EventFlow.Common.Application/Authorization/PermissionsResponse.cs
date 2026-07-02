namespace EventFlow.Common.Application.Authorization;

// Represents the permissions assigned to a user.
public sealed record PermissionsResponse(
    Guid UserId,
    HashSet<string> Permissions);
