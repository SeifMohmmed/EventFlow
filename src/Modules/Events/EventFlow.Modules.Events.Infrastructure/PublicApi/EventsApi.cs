using EventFlow.Common.Domain;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;
using EventFlow.Modules.Events.PublicApi.PublicApi;
using MediatR;
using TicketTypeResponse = EventFlow.Modules.Events.PublicApi.PublicApi.TicketTypeResponse;

namespace EventFlow.Modules.Events.Infrastructure.PublicApi;

// Implements the Events module's public API by delegating requests
// to the application's query handlers through MediatR.
internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypeResponse?> GetTicketTypeAsync(
        Guid ticketTypeId,
        CancellationToken cancellationToken)
    {
        // Execute the query using the application's request pipeline.
        Result<Application.TicketTypes.GetTicketType.TicketTypeResponse> result =
            await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);

        // Return null when the requested ticket type cannot be found
        // or the operation fails.
        if (result.IsFailure)
        {
            return null;
        }

        // Map the internal application response to the public contract
        // to avoid exposing application-specific DTOs across module boundaries.
        return new TicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity);
    }
}
