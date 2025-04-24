using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    public Task AssignRoleToUserAsync(int userId, int roleId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRoleFromUserAsync(int userId, int roleId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Role>> GetRolesByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetUsersByRoleIdAsync(int roleId)
    {
        throw new NotImplementedException();
    }
}