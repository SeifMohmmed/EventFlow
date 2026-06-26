using EventFlow.Common.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventFlow.Common.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that catches unhandled exceptions,
/// logs them, and wraps them in an application-specific exception.
/// </summary>
/// <typeparam name="TRequest">The request being processed.</typeparam>
/// <typeparam name="TResponse">The response returned by the handler.</typeparam>
internal sealed class ExceptionHandlingPipelineBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            // Execute the next behavior or the request handler.
            return await next(cancellationToken);
        }
        catch (Exception exception)
        {
            // Log the unhandled exception with the request name
            // to simplify troubleshooting.
            logger.LogError(
                exception,
                "Unhandled exception for {RequestName}",
                typeof(TRequest).Name);

            // Wrap the original exception in a custom application exception
            // to provide a consistent exception type throughout the application.
            throw new EventFlowException(
                typeof(TRequest).Name,
                innerException: exception);
        }
    }
}
