namespace EventFlow.Modules.Events.PublicApi.PublicApi;

// Defines the public contract exposed by the Events module.
// This allows other modules to retrieve ticket type information
// without coupling to the Events application's internals.
public interface IEventsApi
{
    Task<TicketTypeResponse?> GetTicketTypeAsync(
        Guid ticketTypeId,
        CancellationToken cancellationToken);
}
