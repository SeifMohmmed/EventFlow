using EventFlow.Common.Domain;
using EventFlow.IntegrationTests.Abstractions;
using EventFlow.Modules.Ticketing.Application.Carts.AddItemToCart;
using EventFlow.Modules.Ticketing.Application.Customers.GetCustomer;
using EventFlow.Modules.Users.Application.Users.RegisterUser;
using FluentAssertions;

namespace EventFlow.IntegrationTests.AddToCart;

// End-to-end tests for adding items to a shopping cart.
public sealed class AddItemToCartTests : BaseIntegrationTest
{
    private const decimal Quantity = 10;

    public AddItemToCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Customer_ShouldBeAbleTo_AddItemToCart()
    {
        // Arrange - Register a new user.
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Wait until the user has been propagated to the Ticketing module.
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetCustomerQuery(userResult.Value);

                return await Sender.Send(query);
            });

        customerResult.IsSuccess.Should().BeTrue();

        CustomerResponse customer = customerResult.Value;

        var ticketTypeId = Guid.NewGuid();

        // Create an event with available tickets.
        await Sender.CreateEventAsync(Guid.NewGuid(), ticketTypeId, Quantity);

        // Act - Add tickets to the customer's cart.
        Result result = await Sender.Send(
            new AddItemToCartCommand(
                customer.Id,
                ticketTypeId,
                Quantity));

        // Assert - The item should be added successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
