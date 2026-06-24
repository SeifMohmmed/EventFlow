using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
