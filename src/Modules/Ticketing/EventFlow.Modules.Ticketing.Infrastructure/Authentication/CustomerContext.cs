using EventFlow.Common.Application.Exceptions;
using EventFlow.Common.Infrastructure.Authentication;
using EventFlow.Modules.Ticketing.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace EventFlow.Modules.Ticketing.Infrastructure.Authentication;

internal sealed class CustomerContext(IHttpContextAccessor httpContextAccessor) : ICustomerContext
{
    public Guid CustomerId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EventFlowException("User identifier is unavailable");
}
