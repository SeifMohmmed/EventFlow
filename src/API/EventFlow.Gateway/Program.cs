using EventFlow.Gateway.Authentication;
using EventFlow.Gateway.Middleware;
using EventFlow.Gateway.OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(DiagonosticsConfig.ServiceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("Yarp.ReverseProxy");

        tracing.AddOtlpExporter();
    });

builder.Services.AddAuthorization();

builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.ConfigureOptions<JwtBearerConfigureOptions>();

WebApplication app = builder.Build();

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

await app.RunAsync();
