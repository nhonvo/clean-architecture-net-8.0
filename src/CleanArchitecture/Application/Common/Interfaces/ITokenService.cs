using System.Security.Claims;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
    public ClaimsPrincipal ValidateToken(string token);
}
