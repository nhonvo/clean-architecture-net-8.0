using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Domain.Authorization;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        // Check if the user has the required scope claim with the correct issuer
        var claim = context.User.FindFirst(c => c.Type == "scope" && requirement.Issuer.Contains(c.Issuer));

        if (claim == null)
        {
            return Task.CompletedTask;
        }

        // Split the claim value into individual scopes
        var scopes = claim.Value.Split(" ");

        // Check if any of the user's scopes match the required scope
        if (scopes.Any(scope => scope == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
