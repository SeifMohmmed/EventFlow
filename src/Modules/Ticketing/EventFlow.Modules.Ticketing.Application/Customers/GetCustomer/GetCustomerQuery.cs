using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Ticketing.Application.Customers.GetCustomer;

public sealed record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerResponse>;
