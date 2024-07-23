using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.AuthIdentity.LoginSocial;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller;

public class AuthIdentityController(IAuthIdentityService authIdentityService,
     ILogger<AuthIdentityController> logger) : BaseController
{
    private readonly IAuthIdentityService _authIdentityService = authIdentityService;
    private readonly ILogger _logger = logger;

    //Phương thức thêm Token vào Cookie
    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var response = await _authIdentityService.RefreshTokenAsync(refreshToken);

        return Ok(response);
    }

    [HttpPost("Authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
    {
        var result = await _authIdentityService.Authenticate(request);

        //Lưu Token vào Cookie
        SetRefreshTokenInCookie(result.Token);
        return Ok(result);
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _authIdentityService.Register(request);
        return Ok();
    }

    [HttpPost("ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(string email, string otp, string newPassword)
        => Ok(await _authIdentityService.ResetPassword(email, otp, newPassword));


    [HttpPost("SendPasswordResetCode")]
    [AllowAnonymous]
    public async Task<IActionResult> SendPasswordResetCode(string email)
        => Ok(await _authIdentityService.SendPasswordResetCode(email));

    [HttpPost("SignInFacebook")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInFacebook([FromBody] LoginSocialRequest request)
        => Ok(await _authIdentityService.SignInFacebook(request.AccessToken));

    [HttpPost("SignInGoogle")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInGoogle([FromBody] LoginSocialRequest request)
        => Ok(await _authIdentityService.SignInGoogle(request.AccessToken));

    [HttpPost("SignInApple")]
    [AllowAnonymous]
    public async Task<IActionResult> SignInApple([FromBody] LoginSocialRequest request)
        => Ok(await _authIdentityService.SignInApple(request.FullName, request.AccessToken));
}
