using EventFlow.Common.Application.Messaging;
using EventFlow.Modules.Events.Application.TicketTypes.GetTicketType;

namespace EventFlow.Modules.Events.Application.TicketTypes.GetTicketTypes;

public sealed record GetTicketTypesQuery(
    Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;

