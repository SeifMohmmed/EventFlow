using System.Reflection;
using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EventFlow.Common.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that automatically validates incoming commands
/// before they reach their handlers.
/// </summary>
/// <typeparam name="TRequest">The command being processed.</typeparam>
/// <typeparam name="TResponse">The response returned by the handler.</typeparam>
internal sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Execute all registered FluentValidation validators.
        ValidationFailure[] validationFailures = await ValidateAsync(request);

        // Continue to the request handler if validation succeeds.
        if (validationFailures.Length == 0)
        {
            return await next(cancellationToken);
        }

        // If the handler returns Result<T>, create and return
        // a validation failure result using reflection.
        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.ValidationFailure));

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(
                    null,
                    [CreateValidationError(validationFailures)]);
            }
        }
        // If the handler returns a non-generic Result,
        // return a failure result directly.
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(
                CreateValidationError(validationFailures));
        }

        // For handlers that don't return Result/Result<T>,
        // throw the standard FluentValidation exception.
        throw new ValidationException(validationFailures);
    }

    /// <summary>
    /// Executes all registered validators for the current request
    /// and collects any validation failures.
    /// </summary>
    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        // Skip validation if no validators are registered.
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        // Run all validators in parallel.
        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context)));

        // Flatten all validation errors into a single array.
        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    /// <summary>
    /// Converts FluentValidation failures into the application's
    /// ValidationError object.
    /// </summary>
    private static ValidationError CreateValidationError(
        ValidationFailure[] validationFailures) =>
        new(validationFailures
            .Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage))
            .ToArray());
}
