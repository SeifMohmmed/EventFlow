using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.GetCustomer;
using EventFlow.Modules.Ticketing.Domain.Customers;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Customers;

// Integration tests for retrieving customers.
public class GetCustomerByIdTests : BaseIntegrationTest
{
    public GetCustomerByIdTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange - Use a customer that does not exist.
        var query = new GetCustomerQuery(Guid.NewGuid());

        // Act - Attempt to retrieve the customer.
        Result result = await Sender.Send(query);

        // Assert - The customer should not be found.
        result.Error.Should().Be(CustomerErrors.NotFound(query.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnCustomer_WhenCustomerExists()
    {
        // Arrange - Create a customer.
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var query = new GetCustomerQuery(customerId);

        // Act - Retrieve the customer.
        Result<CustomerResponse> result = await Sender.Send(query);

        // Assert - The customer should be returned successfully.
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}
