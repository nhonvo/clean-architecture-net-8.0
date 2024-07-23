using CleanArchitecture.Application.Common.Models.AuthIdentity.Roles;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IRoleService
{
    Task<List<RoleViewModel>> GetAll();
}