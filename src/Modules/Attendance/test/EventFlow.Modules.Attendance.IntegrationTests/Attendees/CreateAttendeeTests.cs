using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Attendees.CreateAttendee;
using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.IntegrationTests.Attendees;

// Integration tests for attendee creation.
public class CreateAttendeeTests : BaseIntegrationTest
{
    public CreateAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid()
    {
        // Arrange - Build an invalid attendee creation command.
        var command = new CreateAttendeeCommand(
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
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange - Build a valid attendee creation command.
        var command = new CreateAttendeeCommand(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Create the attendee.
        Result result = await Sender.Send(command);

        // Assert - The attendee should be created successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
