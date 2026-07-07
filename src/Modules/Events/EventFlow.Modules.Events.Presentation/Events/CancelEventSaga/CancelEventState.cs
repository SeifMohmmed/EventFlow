using MassTransit;

namespace EventFlow.Modules.Events.Presentation.Events.CancelEventSaga;

// Stores the persisted state of the cancellation saga.
public sealed class CancelEventState : SagaStateMachineInstance, ISagaVersion
{
    // Correlation identifier shared across all related messages.
    public Guid CorrelationId { get; set; }

    // Used by the Redis saga repository for optimistic concurrency.
    public int Version { get; set; }

    // Current state within the state machine.
    public string CurrentState { get; set; }

    // Tracks completion of the composite event.
    public int CancellationCompletedStatus { get; set; }
}
