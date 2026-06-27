using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;

