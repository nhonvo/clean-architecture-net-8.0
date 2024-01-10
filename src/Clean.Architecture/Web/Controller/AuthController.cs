using Clean.Architecture.Application.Common.Interfaces;
using Clean.Architecture.Application.Common.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controller
{
    public class AuthController(IUserService userWriteService) : BaseController
    {
        private readonly IUserService _userWriteService = userWriteService;

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(LoginRequest request)
            => Ok(await _userWriteService.Authenticate(request));

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
            => Ok(await _userWriteService.Register(request));

    }
}
