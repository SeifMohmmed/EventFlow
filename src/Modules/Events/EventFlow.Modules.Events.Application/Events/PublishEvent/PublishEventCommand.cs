using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Events.PublishEvent;

public sealed record PublishEventCommand(
    Guid EventId) : ICommand;
