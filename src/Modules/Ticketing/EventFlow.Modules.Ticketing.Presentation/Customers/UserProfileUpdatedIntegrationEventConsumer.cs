using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.UpdateCustomer;
using EventFlow.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        // Update the customer profile in the Ticketing module.
        Result result = await sender.Send(
            new UpdateCustomerCommand(
                context.Message.UserId,
                context.Message.FirstName,
                context.Message.LastName),
            context.CancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(UpdateCustomerCommand),
                result.Error);
        }
    }
}
