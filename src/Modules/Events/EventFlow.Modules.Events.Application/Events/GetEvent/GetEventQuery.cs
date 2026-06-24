using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;
