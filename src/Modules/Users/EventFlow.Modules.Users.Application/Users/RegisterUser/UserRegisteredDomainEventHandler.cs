using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.PublicApi;
using EventFlow.Modules.Users.Application.Users.GetUser;
using EventFlow.Modules.Users.Domain.Users;
using MediatR;

namespace EventFlow.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, ITicketingApi ticketingApi)
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> result = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (result.IsFailure)
        {
            throw new EventFlowException(nameof(GetUserQuery), result.Error);
        }

        await ticketingApi.CreateCustomerAsync(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName,
            cancellationToken);
    }
}
