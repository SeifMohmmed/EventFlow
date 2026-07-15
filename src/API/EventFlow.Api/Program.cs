using System.Reflection;
using EventFlow.Api.Extensions;
using EventFlow.Api.Middleware;
using EventFlow.Api.OpenTelemetry;
using EventFlow.Common.Application;
using EventFlow.Common.Infrastructure;
using EventFlow.Common.Infrastructure.Configuration;
using EventFlow.Common.Infrastructure.EventBus;
using EventFlow.Common.Presentation.Endpoints;
using EventFlow.Modules.Attendance.Infrastructure;
using EventFlow.Modules.Events.Infrastructure;
using EventFlow.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies = [
    EventFlow.Modules.Events.Application.AssemblyReference.Assembly,
    EventFlow.Modules.Users.Application.AssemblyReference.Assembly,
    EventFlow.Modules.Attendance.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionString("Database")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Cache")!;
var rabbitMqSettings = new RabbitMqSettings(builder.Configuration.GetConnectionStringOrThrow("Queue"));
string mongoConnectionString = builder.Configuration.GetConnectionString("Mongo")!;

builder.Services.AddInfrastructure(
    DiagonosticsConfig.ServiceName,
    [
         EventsModule.ConfigureConsumers(redisConnectionString),
         AttendanceModule.ConfigureConsumers,
         UsersModule.ConfigureConsumers
    ],
    rabbitMqSettings,
    databaseConnectionString,
    redisConnectionString);

builder.Services.AddMongoInfrastructure(mongoConnectionString);

Uri keyCloakHealthUrl = builder.Configuration.GetKeyCloakHealthUrl();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddRabbitMQ(rabbitConnectionString: rabbitMqSettings.Host)
    .AddMongoDb()
    .AddKeyCloak(keyCloakHealthUrl);

builder.Configuration.AddModuleConfiguration(["users", "events", "attendance"]);

builder.Services.AddEventModule(builder.Configuration);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddAttendanceModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

await app.RunAsync();
