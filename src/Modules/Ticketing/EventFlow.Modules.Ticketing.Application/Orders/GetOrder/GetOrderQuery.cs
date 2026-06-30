using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Orders.GetOrder;

public sealed record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;
