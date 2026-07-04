using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Application.Abstractions.Payments;
using EventFlow.Modules.Ticketing.Domain.Payments;

namespace EventFlow.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class PaymentPartiallyRefundedDomainEventHandler(IPaymentService paymentService)
    : DomainEventHandler<PaymentPartiallyRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentPartiallyRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Refund the specified amount through the payment provider.
        await paymentService.RefundAsync(
            domainEvent.TransactionId,
            domainEvent.RefundAmount);
    }
}
