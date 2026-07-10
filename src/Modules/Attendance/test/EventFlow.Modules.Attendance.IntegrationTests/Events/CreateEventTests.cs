using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Events.CreateEvent;
using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
using FluentAssertions;

namespace EventFlow.Modules.Attendance.IntegrationTests.Events;

// Integration tests for event creation.
public class CreateEventTests : BaseIntegrationTest
{
    public CreateEventTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    // Invalid commands used to verify validation rules.
    public static readonly TheoryData<Guid, string, string, string, DateTime, DateTime?> InvalidData = new()
    {
        { Guid.Empty, Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), string.Empty, Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), string.Empty, Faker.Address.StreetAddress(), default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), string.Empty, default, default },
        { Guid.NewGuid(), Faker.Music.Genre(), Faker.Music.Genre(), Faker.Address.StreetAddress(), default, default }
    };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid(
        Guid eventId,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        // Arrange - Build an invalid event creation command.
        var command = new CreateEventCommand(
            eventId,
            title,
            description,
            location,
            startsAtUtc,
            endsAtUtc);

        // Act - Execute the command.
        Result result = await Sender.Send(command);

        // Assert - Validation should fail.
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange - Build a valid event creation command.
        var eventId = Guid.NewGuid();

        var command = new CreateEventCommand(
            eventId,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            DateTime.UtcNow.AddMinutes(10),
            null);

        // Act - Create the event.
        Result result = await Sender.Send(command);

        // Assert - The event should be created successfully.
        result.IsSuccess.Should().BeTrue();
    }
}
