using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Tickets.CreateTicket;
using EventFlow.Modules.Ticketing.IntegrationEvents;
using MassTransit;
using MediatR;

namespace EventFlow.Modules.Attendance.Presentation.Tickets;

public sealed class TicketIssuedIntegrationEventConsumer(ISender sender)
    : IConsumer<TicketIssuedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<TicketIssuedIntegrationEvent> context)
    {
        // Create the ticket in the Attendance module.
        Result result = await sender.Send(
            new CreateTicketCommand(
                context.Message.TicketId,
                context.Message.CustomerId,
                context.Message.EventId,
                context.Message.Code),
            context.CancellationToken);

        // Propagate the error if the command failed.
        if (result.IsFailure)
        {
            throw new EventFlowException(
                nameof(CreateTicketCommand),
                result.Error);
        }
    }
}
