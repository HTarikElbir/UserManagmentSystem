using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<List<Role>> GetAllRolesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Role> GetRoleByIdAsync(int roleId)
    {
        throw new NotImplementedException();
    }

    public Task<Role> GetRoleByNameAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task AddRoleAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRoleAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRoleAsync(int roleId)
    {
        throw new NotImplementedException();
    }
}