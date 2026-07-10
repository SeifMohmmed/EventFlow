using EventFlow.Common.Domain;
using EventFlow.Modules.Ticketing.Application.Customers.CreateCustomer;
using EventFlow.Modules.Ticketing.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Ticketing.IntegrationTests.Customers;

// Integration tests for customer creation.
public class CreateCustomerTests : BaseIntegrationTest
{
    public CreateCustomerTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid()
    {
        // Arrange - Build an invalid customer creation command.
        var command = new CreateCustomerCommand(
            Guid.NewGuid(),
            string.Empty,
            string.Empty,
            string.Empty);

        // Act - Execute the command.
        Result result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Should_CreateCustomer_WhenCommandIsValid()
    {
        // Arrange - Build a valid customer creation command.
        var command = new CreateCustomerCommand(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Create the customer.
        Result result = await Sender.Send(command);

        // Assert - The customer should be created successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
