using System.Security.Claims;
using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal ValidateToken(string token);
    Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken);
}
