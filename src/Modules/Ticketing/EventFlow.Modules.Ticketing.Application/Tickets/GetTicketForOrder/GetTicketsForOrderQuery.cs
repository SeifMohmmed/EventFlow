using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Ticketing.Application.Tickets.GetTicket;

namespace EventFlow.Modules.Ticketing.Application.Tickets.GetTicketForOrder;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;
