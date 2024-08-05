using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Common.Models.AuthIdentity.Roles;

public class UserRoleViewModel
{
    public UserUpdateRequest EditUser { get; set; }

    public RoleAssignRequest EditRole { get; set; }
}
