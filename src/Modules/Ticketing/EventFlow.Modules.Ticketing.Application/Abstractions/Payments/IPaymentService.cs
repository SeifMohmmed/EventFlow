namespace EventFlow.Modules.Ticketing.Application.Abstractions.Payments;

/// <summary>
/// Defines operations for processing and refunding payments.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Charges the specified amount.
    /// </summary>
    Task<PaymentResponse> ChargeAsync(decimal amount, string currency);

    /// <summary>
    /// Refunds the specified amount for a transaction.
    /// </summary>
    Task RefundAsync(Guid transactionId, decimal amount);
}
