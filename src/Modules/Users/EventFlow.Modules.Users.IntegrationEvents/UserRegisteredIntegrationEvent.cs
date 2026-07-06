using EventFlow.Common.Application.EventBus;

namespace EventFlow.Modules.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent : IntegrationEvent
{
    //Event-Carried State Transfer
    public UserRegisteredIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        string email,
        string firstName,
        string lastName)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }
}
