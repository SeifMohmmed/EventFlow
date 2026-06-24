using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(
    Guid EventId,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand;
