using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;
