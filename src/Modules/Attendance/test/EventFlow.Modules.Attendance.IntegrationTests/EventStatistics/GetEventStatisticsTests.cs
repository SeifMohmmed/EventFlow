#pragma warning disable
//using EventFlow.Common.Domain;
//using EventFlow.Modules.Attendance.Application.EventStatistics.GetEventStatistics;
//using EventFlow.Modules.Attendance.Domain.Events;
//using EventFlow.Modules.Attendance.IntegrationTests.Abstractions;
//using FluentAssertions;

//namespace EventFlow.Modules.Attendance.IntegrationTests.EventStatistics;

//// Integration tests for retrieving event statistics.
//public class GetEventStatisticsTests : BaseIntegrationTest
//{
//    public GetEventStatisticsTests(IntegrationTestWebAppFactory factory)
//        : base(factory)
//    {
//    }

//    [Fact]
//    public async Task Should_ReturnFailure_WhenEventStatisticsDoesNotExist()
//    {
//        // Arrange - Use an event that has no statistics.
//        var query = new GetEventStatisticsQuery(Guid.NewGuid());

//        // Act - Attempt to retrieve the statistics.
//        Result<Application.EventStatistics.EventStatistics> result = await Sender.Send(query);

//        // Assert - The statistics should not be found.
//        result.Error.Should().Be(EventErrors.NotFound(query.EventId));
//    }
//}
