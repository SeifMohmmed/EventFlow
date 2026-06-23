using EventFlow.Modules.Events.Domain.Abstractions;

namespace EventFlow.Modules.Events.Domain;

public sealed class Event : Entity
{
    public Event()
    {

    }
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Location { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus Status { get; private set; }

    public static Event Create(
        string title,
        string description,
        string location,
        DateTime startAtUtc,
        DateTime? endAtUtc)
    {
        var @event = new Event
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startAtUtc,
            EndsAtUtc = endAtUtc
        };

        @event.Raise(new EventCreatedDomainEvent(@event.Id));

        return @event;
    }
}
