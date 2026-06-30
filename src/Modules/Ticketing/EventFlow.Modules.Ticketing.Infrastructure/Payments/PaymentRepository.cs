using EventFlow.Modules.Ticketing.Domain.Events;
using EventFlow.Modules.Ticketing.Domain.Payments;
using EventFlow.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EventFlow.Modules.Ticketing.Infrastructure.Payments;

/// <summary>
/// Provides Entity Framework Core persistence
/// for <see cref="Payment"/> entities.
/// </summary>
internal sealed class PaymentRepository(TicketingDbContext context) : IPaymentRepository
{
    /// <summary>
    /// Retrieves a payment by its identifier.
    /// </summary>
    public async Task<Payment?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Payments
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves all payments associated with the specified event.
    /// </summary>
    public async Task<IEnumerable<Payment>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default)
    {
        // Join orders, payments, order items, and ticket types
        // to find all payments made for the specified event.
        return await (
            from order in context.Orders
            join payment in context.Payments
                on order.Id equals payment.OrderId
            join orderItem in context.OrderItems
                on order.Id equals orderItem.OrderId
            join ticketType in context.TicketTypes
                on orderItem.TicketTypeId equals ticketType.Id
            where ticketType.EventId == @event.Id
            select payment)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a new payment to the database context.
    /// </summary>
    public void Insert(Payment payment)
    {
        context.Payments.Add(payment);
    }
}
