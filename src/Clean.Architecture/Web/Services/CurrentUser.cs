using System.Security.Claims;
using Clean.Architecture.Application.Common.Interfaces;

namespace Clean.Architecture.Web.Services;

public class CurrentUser : IUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var Id = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = string.IsNullOrEmpty(Id) ? 0 : int.Parse(Id);

    }
    public int? UserId { get; }
}
