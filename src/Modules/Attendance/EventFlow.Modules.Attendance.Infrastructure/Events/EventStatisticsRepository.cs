using EventFlow.Modules.Attendance.Application.EventStatistics;
using MongoDB.Driver;

namespace EventFlow.Modules.Attendance.Infrastructure.Events;

// MongoDB repository for the Event Statistics projection.
internal sealed class EventStatisticsRepository : IEventStatisticsRepository
{
    private readonly IMongoCollection<EventStatistics> _collection;

    public EventStatisticsRepository(IMongoClient mongoClient)
    {
        // Connect to the Attendance MongoDB database.
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(DocumentDbSettings.Database);

        // Get the Event Statistics collection.
        _collection = mongoDatabase.GetCollection<EventStatistics>(DocumentDbSettings.EventStatistics);
    }

    public async Task<EventStatistics> GetAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        // Find the projection by its EventId.
        FilterDefinition<EventStatistics> filter =
            Builders<EventStatistics>.Filter.Eq(e => e.EventId, eventId);

        EventStatistics eventStatistics =
            await _collection.Find(filter).SingleAsync(cancellationToken);

        return eventStatistics;
    }

    public async Task InsertAsync(
        EventStatistics eventStatistics,
        CancellationToken cancellationToken = default)
    {
        // Insert a new projection document.
        await _collection.InsertOneAsync(
            eventStatistics,
            cancellationToken: cancellationToken);
    }

    public async Task ReplaceAsync(
        EventStatistics eventStatistics,
        CancellationToken cancellationToken = default)
    {
        // Match the existing document by EventId.
        FilterDefinition<EventStatistics> filter = Builders<EventStatistics>
            .Filter
            .Eq(e => e.EventId, eventStatistics.EventId);

        // Replace the existing projection with the updated version.
        await _collection.ReplaceOneAsync(
            filter,
            eventStatistics,
            cancellationToken: cancellationToken);
    }
}
