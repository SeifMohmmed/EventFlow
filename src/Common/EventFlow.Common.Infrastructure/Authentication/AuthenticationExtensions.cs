using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    /// <summary>
    /// Registers the application's authentication and authorization services.
    /// This includes:
    /// - Authorization services.
    /// - JWT Bearer authentication.
    /// - HttpContext accessor.
    /// - Configuration binding for <see cref="JwtBearerOptions"/>.
    /// </summary>
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        // Register ASP.NET Core authorization services.
        services.AddAuthorization();

        // Register JWT Bearer authentication.
        // The options are configured separately by JwtBearerConfigureOptions.
        services.AddAuthentication().AddJwtBearer();

        // Allows services to access the current HttpContext.
        services.AddHttpContextAccessor();

        // Bind JwtBearerOptions from configuration.
        services.ConfigureOptions<JwtBearerConfigureOptions>();

        return services;
    }
}
