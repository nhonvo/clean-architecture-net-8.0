using Clean.Architecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controller
{
    public class SeedController(ISeedService seedService) : BaseController
    {
        private readonly ISeedService _seedService = seedService;

        [HttpGet]
        public async Task<IActionResult> Seed(CancellationToken token)
        {
            await _seedService.Seed(token);
            return NoContent();
        }
    }
}
