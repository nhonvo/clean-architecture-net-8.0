using CleanArchitecture.Shared.Models.AuthIdentity.Roles;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRoleService
{
    Task<List<RoleViewModel>> GetAll();
}
