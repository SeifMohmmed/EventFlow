using System.Reflection;
using EventFlow.Common.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Application;

/// <summary>
/// Provides extension methods for registering
/// application-layer services.
/// </summary>
public static class ApplicationConfiguration
{
    /// <summary>
    /// Registers MediatR, pipeline behaviors, and FluentValidation
    /// validators from the specified module assemblies.
    /// </summary>
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        Assembly[] moduleAssemblies)
    {
        // Register MediatR handlers and configure the request pipeline.
        services.AddMediatR(cfg =>
        {
            // Discover handlers from all module assemblies.
            cfg.RegisterServicesFromAssemblies(moduleAssemblies);

            // Register pipeline behaviors in execution order.
            cfg.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        // Automatically discover and register all FluentValidation validators.
        services.AddValidatorsFromAssemblies(
            moduleAssemblies,
            includeInternalTypes: true);

        return services;
    }
}
