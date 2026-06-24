
using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Events.CancelEvent;

public sealed record CancelEventCommand(
    Guid EventId) : ICommand;
