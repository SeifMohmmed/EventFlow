using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Attendees.UpdateAttendee;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.IntegrationTests.Attendees;

// Integration tests for updating attendees.
public class UpdateAttendeeTests : BaseIntegrationTest
{
    public UpdateAttendeeTests(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange - Use an attendee that does not exist.
        var command = new UpdateAttendeeCommand(
            Guid.NewGuid(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Attempt to update the attendee.
        Result result = await Sender.Send(command);

        // Assert - The attendee should not be found.
        result.Error.Should().Be(AttendeeErrors.NotFound(command.AttendeeId));
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeExists()
    {
        // Arrange - Create an attendee.
        Guid attendeeId = await Sender.CreateAttendeeAsync(Guid.NewGuid());

        var command = new UpdateAttendeeCommand(
            attendeeId,
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act - Update the attendee.
        Result result = await Sender.Send(command);

        // Assert - The update should succeed.
        result.IsSuccess.Should().BeTrue();
    }
}
