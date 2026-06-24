using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Events.GetEvents;

public sealed record GetEventsQuery
    : IQuery<IReadOnlyCollection<EventResponse>>;
