using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Events.GetEvents;

public sealed record GetEventsQuery
    : IQuery<IReadOnlyCollection<EventResponse>>;
