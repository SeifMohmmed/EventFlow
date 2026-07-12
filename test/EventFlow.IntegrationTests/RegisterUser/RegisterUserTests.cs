#pragma warning disable S125
/*using EventFlow.Common.Domain;
using EventFlow.IntegrationTests.Abstractions;
using EventFlow.Modules.Attendance.Application.Attendees.GetAttendee;
using EventFlow.Modules.Ticketing.Application.Customers.GetCustomer;
using EventFlow.Modules.Users.Application.Users.RegisterUser;
using FluentAssertions;

namespace EventFlow.IntegrationTests.RegisterUser;

// End-to-end tests verifying user registration is propagated across modules.
public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToTicketingModule()
    {
        // Arrange - Register a new user.
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Wait until the Ticketing module creates the corresponding customer.
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                return await Sender.Send(new GetCustomerQuery(userResult.Value));
            });

        // Assert - The customer should exist.
        customerResult.IsSuccess.Should().BeTrue();
        customerResult.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToAttendanceModule()
    {
        // Arrange - Register a new user.
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Wait until the Attendance module creates the corresponding attendee.
        Result<AttendeeResponse> attendeeResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                var query = new GetAttendeeQuery(userResult.Value);

                Result<AttendeeResponse> customerResult = await Sender.Send(query);

                return customerResult;
            });

        // Assert - The attendee should exist.
        attendeeResult.IsSuccess.Should().BeTrue();
        attendeeResult.Value.Should().NotBeNull();
    }
}
*/
