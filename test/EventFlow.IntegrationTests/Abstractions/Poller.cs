using EventFlow.Common.Domain;

namespace EventFlow.IntegrationTests.Abstractions;

// Utility for waiting until an eventually consistent operation completes.
internal static class Poller
{
    private static readonly Error Timeout =
        Error.Failure("Poller.Timeout", "The poller has time out");

    internal static async Task<Result<T>> WaitAsync<T>(
        TimeSpan timeout,
        Func<Task<Result<T>>> func)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        DateTime endTimeUtc = DateTime.UtcNow.Add(timeout);

        // Retry until the operation succeeds or the timeout expires.
        while (DateTime.UtcNow < endTimeUtc &&
               await timer.WaitForNextTickAsync())
        {
            Result<T> result = await func();

            if (result.IsSuccess)
            {
                return result;
            }
        }

        return Result.Failure<T>(Timeout);
    }
}
