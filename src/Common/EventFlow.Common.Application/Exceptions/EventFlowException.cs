
using EventFlow.Common.Domain;

namespace EventFlow.Common.Application.Exceptions;

/// <summary>
/// Represents an application-specific exception that provides
/// additional context about the request that caused the failure.
/// </summary>
public sealed class EventFlowException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventFlowException"/> class.
    /// </summary>
    /// <param name="requestName">The name of the request that failed.</param>
    /// <param name="error">An optional application error associated with the exception.</param>
    /// <param name="innerException">The original exception that caused this exception.</param>
    public EventFlowException(
        string requestName,
        Error? error = default,
        Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    /// <summary>
    /// Gets the name of the request that caused the exception.
    /// </summary>
    public string RequestName { get; }

    /// <summary>
    /// Gets the application-specific error associated with the exception, if any.
    /// </summary>
    public Error? Error { get; }
}
