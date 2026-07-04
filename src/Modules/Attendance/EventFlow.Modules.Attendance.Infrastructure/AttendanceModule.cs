using EventFlow.Common.Application.Messaging;
using EventFlow.Common.Infrastructure.Outbox;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Attendance.Application.Abstractions.Authentication;
using EventFlow.Modules.Attendance.Application.Abstractions.Data;
using EventFlow.Modules.Attendance.Domain.Attendees;
using EventFlow.Modules.Attendance.Domain.Events;
using EventFlow.Modules.Attendance.Domain.Tickets;
using EventFlow.Modules.Attendance.Infrastructure.Attendees;
using EventFlow.Modules.Attendance.Infrastructure.Authentication;
using EventFlow.Modules.Attendance.Infrastructure.Database;
using EventFlow.Modules.Attendance.Infrastructure.Events;
using EventFlow.Modules.Attendance.Infrastructure.Outbox;
using EventFlow.Modules.Attendance.Infrastructure.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    public static IServiceCollection AddAttendanceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Attendance))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AttendanceDbContext>());

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<IAttendanceContext, AttendanceContext>();

        // Bind Attendance module outbox settings from configuration.
        services.Configure<OutboxOptions>(
            configuration.GetSection("Attendance:Outbox"));

        // Register Quartz configuration for the outbox processing job.
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        // Discover all domain event handlers in the Application assembly.
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            // Register the original handler.
            services.TryAddScoped(domainEventHandler);

            // Determine the domain event handled by this handler.
            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            // Create the matching IdempotentDomainEventHandler<T>.
            Type closedIdempotentHandler =
                typeof(IdempotentDomainEventHandler<>)
                    .MakeGenericType(domainEvent);

            // Decorate the original handler with the idempotent wrapper.
            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}
