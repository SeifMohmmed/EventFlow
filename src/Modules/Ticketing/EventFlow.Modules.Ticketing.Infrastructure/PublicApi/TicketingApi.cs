using EventFlow.Modules.Ticketing.Application.Customers.CreateCustomer;
using EventFlow.Modules.Ticketing.PublicApi;
using MediatR;

namespace EventFlow.Modules.Ticketing.Infrastructure.PublicApi;

// Exposes the Ticketing module's public API.
// Other modules interact with Ticketing through this abstraction
// instead of referencing its application layer directly.
internal sealed class TicketingApi(ISender sender) : ITicketingApi
{
    public async Task CreateCustomerAsync(
        Guid customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        // Delegate the request to the application's command handler,
        // allowing the request to flow through the MediatR pipeline.
        await sender.Send(
            new CreateCustomerCommand(customerId, email, firstName, lastName),
            cancellationToken);
    }
}
