using EventFlow.Api.Extensions;
using EventFlow.Modules.Api.Events;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEventModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Enables Swagger UI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "EventFlow API v1");
    });

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

await app.RunAsync();
