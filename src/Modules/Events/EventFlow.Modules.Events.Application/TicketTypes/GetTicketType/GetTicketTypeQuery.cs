using EventFlow.Modules.Events.Application.Abstractions.Messaging;
using EventFlow.Modules.Events.Application.Events.GetEvent;

namespace EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
