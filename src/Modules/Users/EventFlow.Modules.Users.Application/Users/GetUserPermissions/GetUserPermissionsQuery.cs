using EventFlow.Common.Application.Authorization;
using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Users.Application.Users.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;
