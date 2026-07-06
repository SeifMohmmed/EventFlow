using EventFlow.Common.Application.EventBus;
using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.CreateCustomer;
using EventFlow.Modules.Users.IntegrationEvents;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Customers;

internal sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        //Event-Carried State Transfer
        Result result = await sender.Send(
            new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName),
            cancellationToken);

        // Fail the message if the customer could not be created.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(CreateCustomerCommand),
                result.Error);
        }
    }
}
