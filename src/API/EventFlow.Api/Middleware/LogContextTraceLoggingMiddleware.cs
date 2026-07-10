using System.Diagnostics;
using Serilog.Context;

namespace EventFlow.Api.Middleware;

// Adds the current distributed trace identifier to the Serilog log context.
internal sealed class LogContextTraceLoggingMiddleware(RequestDelegate next)
{
    public Task Invoke(HttpContext context)
    {
        // Retrieve the current OpenTelemetry trace identifier.
        string traceId = Activity.Current?.TraceId.ToString();

        // Enrich all logs generated during this request with the trace identifier.
        using (LogContext.PushProperty("TraceId", traceId))
        {
            return next(context);
        }
    }
}
