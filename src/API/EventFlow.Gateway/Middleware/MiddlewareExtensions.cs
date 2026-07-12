namespace EventFlow.Gateway.Middleware;

// Extension methods for registering custom middleware.
internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseLogContextTraceLogging(this IApplicationBuilder app)
    {
        // Add middleware that enriches Serilog logs with the current trace identifier.
        app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        return app;
    }
}
