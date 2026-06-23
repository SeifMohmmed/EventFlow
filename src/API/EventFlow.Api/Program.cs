using EventFlow.Api.Extensions;
using EventFlow.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer(); // <-- add this
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

await app.RunAsync();
