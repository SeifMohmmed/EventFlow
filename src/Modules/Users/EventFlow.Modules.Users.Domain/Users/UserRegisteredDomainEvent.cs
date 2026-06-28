using EventFlow.Common.Domain;

namespace EventFlow.Modules.Users.Domain.Users;

// Raised after a new user has been successfully registered.
// Other modules can react to this event without introducing
// direct dependencies on the Users module.
public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
}
