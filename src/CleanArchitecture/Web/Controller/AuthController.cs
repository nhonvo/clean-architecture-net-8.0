using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller
{
    public class AuthController(IUserService userWriteService) : BaseController
    {
        private readonly IUserService _userService = userWriteService;

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(UserSignInRequest request)
            => Ok(await _userService.SignIn(request));

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserSignUpRequest request, CancellationToken token)
            => Ok(await _userService.SignUp(request, token));

        [HttpDelete("logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Ok();
        }

        [HttpGet("refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
            => Ok(await _userService.RefreshToken());
    }
}
