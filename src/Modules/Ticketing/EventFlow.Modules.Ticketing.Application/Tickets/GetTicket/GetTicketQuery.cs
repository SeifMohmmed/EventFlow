using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Tickets.GetTicket;

public sealed record GetTicketQuery(Guid TicketId) : IQuery<TicketResponse>;
