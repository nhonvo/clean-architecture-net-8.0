using Clean.Architecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Clean.Architecture.Domain.Constant;

namespace Clean.Architecture.Web.Controller
{
    public class SeedController(ISeedService seedService) : BaseController
    {
        private readonly ISeedService _seedService = seedService;

        [HttpGet]
        public async Task<IActionResult> Seed()
        {
            await _seedService.Seed();
            return Ok(SeedingMessage.SeedDataSuccessMessage);
        }
    }
}
