using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
