using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace EventFlow.Common.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider
    : DefaultAuthorizationPolicyProvider
{
    // Provides access to the application's authorization options.
    private readonly AuthorizationOptions _authorizationOptions;

    public PermissionAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options)
        : base(options)
    {
        // Store the authorization options so new policies can be added.
        _authorizationOptions = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(
        string policyName)
    {
        // Try to retrieve an existing policy.
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        // Return it if it already exists.
        if (policy is not null)
        {
            return policy;
        }

        // Create a new policy that requires the permission matching the policy name.
        AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        // Cache the generated policy for future requests.
        _authorizationOptions.AddPolicy(policyName, permissionPolicy);

        // Return the newly created policy.
        return permissionPolicy;
    }
}
