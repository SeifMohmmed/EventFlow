namespace EventFlow.Modules.Users.IntegrationEvents;

// Request message used to retrieve a user's permissions.
public sealed record GetUserPermissionsRequest(string IdentityId);
