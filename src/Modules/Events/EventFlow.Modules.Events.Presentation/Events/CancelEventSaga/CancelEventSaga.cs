using EventFlow.Modules.Events.IntegrationEvents;
using EventFlow.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace EventFlow.Modules.Events.Presentation.Events.CancelEventSaga;

// Coordinates the distributed event cancellation workflow.
public sealed class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    public State CancellationStarted { get; private set; }
    public State PaymentsRefunded { get; private set; }
    public State TicketsArchived { get; private set; }

    public Event<EventCanceledIntegrationEvent> EventCanceled { get; private set; }
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; }
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; }

    public Event EventCancellationCompleted { get; set; }

    public CancelEventSaga()
    {
        // Correlate incoming messages using the EventId.
        Event(() => EventCanceled, m => m.CorrelateById(c => c.Message.EventId));
        Event(() => EventPaymentsRefunded, m => m.CorrelateById(c => c.Message.EventId));
        Event(() => EventTicketsArchived, m => m.CorrelateById(c => c.Message.EventId));

        // Persist the current saga state.
        InstanceState(s => s.CurrentState);

        Initially(
            When(EventCanceled)
                // Notify downstream services that cancellation has started.
                .Publish(context =>
                    new EventCancellationStartedIntegrationEvent(
                        context.Message.Id,
                        context.Message.OccurredOnUtc,
                        context.Message.EventId))
                .TransitionTo(CancellationStarted));

        During(CancellationStarted,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded),

            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(PaymentsRefunded,
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(TicketsArchived,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded));

        // Complete the saga only after both required events have been received.
        CompositeEvent(
            () => EventCancellationCompleted,
            state => state.CancellationCompletedStatus,
            EventPaymentsRefunded,
            EventTicketsArchived);

        DuringAny(
            When(EventCancellationCompleted)
                // Broadcast that the distributed cancellation process has finished.
                .Publish(context =>
                    new EventCancellationCompletedIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        context.Saga.CorrelationId))
                .Finalize());
    }
}
