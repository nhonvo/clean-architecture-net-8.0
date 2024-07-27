using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.AuthIdentity.LoginSocial;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller;
public class AuthIdentityController(IAuthIdentityService authIdentityService) : BaseController
{
    private readonly IAuthIdentityService _authIdentityService = authIdentityService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authIdentityService.Authenticate(request, cancellationToken);

        SetTokenInCookie(result.Token);
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        await _authIdentityService.Register(request, cancellationToken);
        return NoContent();
    }

    [HttpGet("refreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = GetTokenInCookie();

        var response = await _authIdentityService.RefreshTokenAsync(refreshToken, cancellationToken);

        return Ok(response);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        => Ok(await _authIdentityService.Get(cancellationToken));

    [HttpPost("resetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        await _authIdentityService.ResetPassword(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("sendPasswordResetCode")]
    [AllowAnonymous]
    public async Task<IActionResult> SendPasswordResetCode(SendPasswordResetCodeRequest request, CancellationToken cancellationToken)
        => Ok(await _authIdentityService.SendPasswordResetCode(request, cancellationToken));

    // [HttpPost("SignInFacebook")]
    // [AllowAnonymous]
    // public async Task<IActionResult> SignInFacebook([FromBody] LoginSocialRequest request, CancellationToken cancellationToken)
    //     => Ok(await _authIdentityService.SignInFacebook(request.AccessToken, cancellationToken));

    // [HttpPost("SignInGoogle")]
    // [AllowAnonymous]
    // public async Task<IActionResult> SignInGoogle([FromBody] LoginSocialRequest request, CancellationToken cancellationToken)
    //     => Ok(await _authIdentityService.SignInGoogle(request.AccessToken, cancellationToken));

    // [HttpPost("SignInApple")]
    // [AllowAnonymous]
    // public async Task<IActionResult> SignInApple([FromBody] LoginSocialRequest request, CancellationToken cancellationToken)
    //     => Ok(await _authIdentityService.SignInApple(request.FullName, request.AccessToken, cancellationToken));

    private string GetTokenInCookie() => Request.Cookies["token_key"];

    private void SetTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("token_key", refreshToken, cookieOptions);
    }
}
