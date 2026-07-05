using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.UpdateCustomer;
using EventFlow.Modules.Users.IntegrationEvents;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(
        UserProfileUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        // Update the customer profile in the Ticketing module.
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(UpdateCustomerCommand),
                result.Error);
        }
    }
}
