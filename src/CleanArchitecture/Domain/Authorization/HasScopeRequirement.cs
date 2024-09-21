using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Domain.Authorization;
public class HasScopeRequirement(string scopeBaseDomain, string scope, string issuer) : IAuthorizationRequirement
{
    public string ScopeBaseDomain { get; } = scopeBaseDomain ?? throw new ArgumentNullException(nameof(scopeBaseDomain));
    public string Scope { get; } = scope ?? throw new ArgumentNullException(nameof(scope));
    public string Issuer { get; } = issuer ?? throw new ArgumentNullException(nameof(issuer));
}
