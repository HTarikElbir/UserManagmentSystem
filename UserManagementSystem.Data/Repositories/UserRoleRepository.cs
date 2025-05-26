using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AssignRoleToUserAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
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