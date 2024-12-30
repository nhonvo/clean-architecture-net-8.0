using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _userService = authService;

    /// <summary>
    /// sign in 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("sign-in")]
    [SwaggerResponse(200, "Successfully signed in.", typeof(UserSignInResponse))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(401, "Authentication failed.")]
    public async Task<IActionResult> SignIn(UserSignInRequest request)
        => Ok(await _userService.SignIn(request));

    /// <summary>
    /// sign up
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("sign-up")]
    [SwaggerResponse(201, "User registered successfully.")]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> SignUp(UserSignUpRequest request, CancellationToken token)
        => Ok(await _userService.SignUp(request, token));

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [HttpDelete("logout")]
    [SwaggerResponse(200, "Successfully logged out.")]
    public IActionResult Logout()
    {
        _userService.Logout();
        return Ok();
    }

    /// <summary>
    /// refresh a token
    /// </summary>
    /// <returns></returns>
    [HttpGet("refresh")]
    [SwaggerResponse(200, "Token refreshed successfully.", typeof(string))]
    [SwaggerResponse(401, "Refresh token is invalid or expired.")]
    public async Task<IActionResult> RefreshToken()
        => Ok(await _userService.RefreshToken());

    /// <summary>
    /// get for profile information
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile")]
    [Authorize]
    [SwaggerResponse(200, "Successfully retrieved user profile.", typeof(UserProfileResponse))]
    [SwaggerResponse(401, "User is not authorized.")]
    public async Task<IActionResult> GetProfile()
        => Ok(await _userService.GetProfile());
}
