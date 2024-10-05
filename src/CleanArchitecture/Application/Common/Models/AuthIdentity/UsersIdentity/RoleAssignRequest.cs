using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class RoleAssignRequest
{
    public string UserId { get; set; }

    public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    public List<SelectItem> Scopes { get; set; }
}
