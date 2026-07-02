using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlow.Common.Infrastructure.Authorization;

internal static class AuthorizationExtensions
{
    internal static IServiceCollection AddAuthorizationInternal(
        this IServiceCollection services)
    {
        // Registers the claims transformation that enriches authenticated users.
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        // Registers the custom permission authorization handler.
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Registers the dynamic permission policy provider.
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
