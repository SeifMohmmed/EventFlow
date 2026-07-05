using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Users.Application.Users.GetUser;
using EventFlow.Modules.Users.Domain.Users;
using EventFlow.Modules.Users.IntegrationEvents;
using MediatR;

namespace EventFlow.Modules.Users.Application.Users.RegisterUser;

/// <summary>
/// Handles the user registered domain event by publishing
/// an integration event for other modules.
/// </summary>
internal sealed class UserRegisteredDomainEventHandler(
    ISender sender,
    IEventBus eventBus)
    : DomainEventHandler<UserRegisteredDomainEvent>
{
    /// <summary>
    /// Publishes a <see cref="UserRegisteredIntegrationEvents"/>
    /// after retrieving the registered user's details.
    /// </summary>
    public override async Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the registered user's details.
        Result<UserResponse> result =
            await sender.Send(
                new GetUserQuery(notification.UserId),
                cancellationToken);

        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(GetUserQuery),
                result.Error);
        }

        // Publish an integration event so other modules
        // can react to the new user.
        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName),
            cancellationToken);
    }
}
