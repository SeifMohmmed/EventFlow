using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Users.IntegrationEvents;

/// <summary>
/// Integration event published when a user's profile is updated.
/// </summary>
public sealed class UserProfileUpdatedIntegrationEvent : IntegrationEvent
{
    public UserProfileUpdatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        string firstName,
        string lastName)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }

    // The updated user's identifier.
    public Guid UserId { get; init; }

    // The user's updated first name.
    public string FirstName { get; init; }

    // The user's updated last name.
    public string LastName { get; init; }
}
