using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.CreateCustomer;
using EventFlow.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EventFlow.Modules.Ticketing.Presentation.Customers;

/// <summary>
/// Consumes user registration integration events
/// and creates a corresponding customer.
/// </summary>
public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IConsumer<UserRegisteredIntegrationEvents>
{
    /// <summary>
    /// Handles the received integration event.
    /// </summary>
    public async Task Consume(
        ConsumeContext<UserRegisteredIntegrationEvents> context)
    {
        Result result = await sender.Send(
            new CreateCustomerCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName));

        // Fail the message if the customer could not be created.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(CreateCustomerCommand),
                result.Error);
        }
    }
}
