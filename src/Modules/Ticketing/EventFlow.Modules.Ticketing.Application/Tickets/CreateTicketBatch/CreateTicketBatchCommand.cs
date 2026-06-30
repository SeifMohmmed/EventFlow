using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
