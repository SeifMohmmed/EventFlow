using System.Diagnostics;
using EventFlow.Common.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace EventFlow.Common.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the execution of every request,
/// including its module, name, and any errors returned by the handler.
/// </summary>
/// <typeparam name="TRequest">The request being processed.</typeparam>
/// <typeparam name="TResponse">The response returned by the handler.</typeparam>
internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Determine the module and request names for structured logging.
        string moduleName = GetModuleName(typeof(TRequest).FullName!);
        string requestName = typeof(TRequest).Name;

        // Add request metadata to the current distributed trace.
        Activity.Current?.SetTag("request.module", moduleName);
        Activity.Current?.SetTag("request.name", requestName);

        // Add the module name as a Serilog property so it appears
        // in every log entry generated during this request.
        using (LogContext.PushProperty("Module", moduleName))
        {
#pragma warning disable CA1873
            // Log the start of request processing.
            logger.LogInformation("Processing request {RequestName}", requestName);

            // Execute the next behavior or the request handler.
            TResponse result = await next(cancellationToken);

            if (result.IsSuccess)
            {
#pragma warning disable CA1873
                // Log successful completion.
                logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                // Include the error details in the logging context.
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError(
                        "Completed request {RequestName} with error",
                        requestName);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Extracts the module name from the request's fully qualified type name.
    /// Example:
    /// EventFlow.Modules.Events.Application.Commands.CreateEventCommand
    /// → Events
    /// </summary>
    private static string GetModuleName(string requestName) =>
        requestName.Split('.')[2];
}
