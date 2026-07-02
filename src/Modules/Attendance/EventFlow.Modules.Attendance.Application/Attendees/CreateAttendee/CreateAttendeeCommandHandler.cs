using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using EventFlow.Modules.Attendance.Application.Abstractions.Data;
using EventFlow.Modules.Attendance.Domain.Attendees;

namespace EventFlow.Modules.Attendance.Application.Attendees.CreateAttendee;

internal sealed class CreateAttendeeCommandHandler(IAttendeeRepository attendeeRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAttendeeCommand>
{
    public async Task<Result> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
    {
        var attendee = Attendee.Create(request.AttendeeId, request.Email, request.FirstName, request.LastName);

        attendeeRepository.Insert(attendee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
