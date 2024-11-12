using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Shared.Models.AuthIdentity.Roles;

public class UserRoleViewModel
{
    public UserUpdateRequest EditUser { get; set; }

    public RoleAssignRequest EditRole { get; set; }
}
