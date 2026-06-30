using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
