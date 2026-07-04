using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Application.Abstractions.Payments;
using EventFlow.Modules.Ticketing.Domain.Payments;

namespace EventFlow.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class PaymentRefundedDomainEventHandler(IPaymentService paymentService)
    : DomainEventHandler<PaymentRefundedDomainEvent>
{
    public override async Task Handle(
        PaymentRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // Refund the specified amount through the payment provider.
        await paymentService.RefundAsync(
            domainEvent.TransactionId,
            domainEvent.RefundAmount);
    }
}
