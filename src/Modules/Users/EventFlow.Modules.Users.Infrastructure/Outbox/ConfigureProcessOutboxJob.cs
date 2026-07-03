using Microsoft.Extensions.Options;
using Quartz;

namespace EventFlow.Modules.Users.Infrastructure.Outbox;

internal sealed class ConfigureProcessOutboxJob(
    IOptions<OutboxOptions> outboxOptions)
    : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public void Configure(QuartzOptions options)
    {
        // Use the job's full type name as its identifier.
        string jobName = typeof(ProcessOutboxJob).FullName!;

        options
            // Register the background job.
            .AddJob<ProcessOutboxJob>(
                configure => configure.WithIdentity(jobName))

            // Schedule the job to run continuously.
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithIntervalInSeconds(_outboxOptions.IntervalInSeconds)
                            .RepeatForever()));
    }
}
