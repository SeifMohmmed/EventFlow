namespace EventFlow.Common.Domain;

/// <summary>
/// Represents one or more validation errors.
/// </summary>
public sealed record ValidationError : Error
{
    /// <summary>
    /// Initializes a new validation error.
    /// </summary>
    public ValidationError(Error[] errors)
        : base(
            "General.Validation",
            "One or more validation errors occurred",
            ErrorType.Validation)
    {
        Errors = errors;
    }

    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public Error[] Errors { get; }

    /// <summary>
    /// Creates a validation error from multiple failed results.
    /// </summary>
    public static ValidationError FromResults(IEnumerable<Result> results) =>
        new(results
            .Where(r => r.IsFailure)
            .Select(r => r.Error)
            .ToArray());
}
