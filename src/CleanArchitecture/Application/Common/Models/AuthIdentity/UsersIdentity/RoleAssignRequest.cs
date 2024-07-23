using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class RoleAssignRequest
{
    public Guid Id { get; set; }

    public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
}
