using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(
    Guid EventId,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand;
