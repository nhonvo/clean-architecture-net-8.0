using System.Security.Claims;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ITokenService _tokenService = tokenService;

    public int GetCurrentUserId()
    {
        var jwtCookie = _httpContextAccessor.HttpContext.Request.Cookies["acc"];
        if (string.IsNullOrEmpty(jwtCookie))
        {
            throw new UserFriendlyException(ErrorCode.Unauthorized, "user not logged in");
        }
        var token = _tokenService.ValidateToken(jwtCookie);
        var userId = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return int.Parse(userId);
    }
}
