using EventFlow.Common.Domain;

namespace EventFlow.Modules.Events.Presentation.ApiResults;

/// <summary>
/// Provides helper methods for mapping application
/// results to HTTP responses.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes the appropriate delegate based on whether
    /// the result represents success or failure.
    /// </summary>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess()
            : onFailure(result);
    }

    /// <summary>
    /// Executes the appropriate delegate based on whether
    /// the result represents success or failure.
    /// </summary>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result);
    }
}
