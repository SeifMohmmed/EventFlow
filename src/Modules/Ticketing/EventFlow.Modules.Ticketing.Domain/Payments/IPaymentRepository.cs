using EventFlow.Modules.Ticketing.Domain.Events;

namespace EventFlow.Modules.Ticketing.Domain.Payments;

/// <summary>
/// Defines persistence operations for <see cref="Payment"/> entities.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Retrieves a payment by its identifier.
    /// </summary>
    Task<Payment?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all payments associated with the specified event.
    /// </summary>
    Task<IEnumerable<Payment>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new payment to the repository.
    /// </summary>
    void Insert(Payment payment);
}
