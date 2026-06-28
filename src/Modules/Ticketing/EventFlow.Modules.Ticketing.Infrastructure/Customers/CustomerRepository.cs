using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Ticketing.Infrastructure.Customers;

internal sealed class CustomerRepository(TicketingDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Retrieve the customer by its identifier.
        return await context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Customer customer)
    {
        // Track the new customer so it will be persisted
        // when the current unit of work is committed.
        context.Customers.Add(customer);
    }
}
