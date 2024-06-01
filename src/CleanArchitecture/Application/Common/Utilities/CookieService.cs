using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Common.Utilities;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public void Set(string token) => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token_key", token, new CookieOptions
    {
        HttpOnly = true,
        SameSite = SameSiteMode.None,
        Secure = true,
        MaxAge = TimeSpan.FromMinutes(30)
    });

    public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token_key");

    public string Get()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["token_key"];
        return string.IsNullOrEmpty(token) ? throw UserException.UserUnauthorizedException() : token;
    }
}
