using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Application.Tickets.GetTicket;

namespace EventFlow.Modules.Ticketing.Application.Tickets.GetTicketByCode;

public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketResponse>;
