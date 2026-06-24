
using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Events.CancelEvent;

public sealed record CancelEventCommand(
    Guid EventId) : ICommand;
