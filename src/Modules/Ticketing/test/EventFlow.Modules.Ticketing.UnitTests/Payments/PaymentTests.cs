using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.Domain.Orders;
using EventFlow.Modules.Ticketing.Domain.Payments;
using EventFlow.Modules.Ticketing.UnitTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.UnitTests.Payments;

// Unit tests for the Payment aggregate.
public class PaymentTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentIsCreated()
    {
        // Arrange - Create a customer and order.
        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        // Act - Create a payment.
        Result<Payment> result = Payment.Create(
            order,
            Guid.NewGuid(),
            Faker.Random.Decimal(),
            Faker.Random.String(3));

        // Assert - Verify the payment created domain event was published.
        PaymentCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(result.Value);

        domainEvent.PaymentId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenAlreadyRefunded()
    {
        // Arrange - Create and refund a payment.
        decimal amount = Faker.Random.Decimal();

        var customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        var order = Order.Create(customer);

        Result<Payment> paymentResult = Payment.Create(
            order,
            Guid.NewGuid(),
            amount,
            Faker.Random.String(3));

        Payment payment = paymentResult.Value;

        payment.Refund(amount);

        // Act - Attempt to refund the payment again.
        Result result = payment.Refund(amount);

        // Assert - Verify the payment cannot be refunded twice.
        result.Error.Should().Be(PaymentErrors.AlreadyRefunded);
    }
}
