using EventFlow.Common.Infrastructure.Interceptors;
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
using EventFlow.Modules.Attendance.Infrastructure.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    public static IServiceCollection AddAttendanceModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AttendanceDbContext>());

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<IAttendanceContext, AttendanceContext>();
    }
}
