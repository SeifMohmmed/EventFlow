using EventFlow.Modules.Events.Application.Abstractions.Data;
using EventFlow.Modules.Events.Domain;
using EventFlow.Modules.Events.Domain.Events;
using MediatR;

namespace EventFlow.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventsCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateEventCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateEventCommand request,
        CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        eventRepository.Insert(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return @event.Id;
    }
}
