using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // Returns a list of all roles in the system.
    public async Task<List<Role>> GetAllRolesAsync() => await  _context.Roles
        .AsNoTracking()
        .ToListAsync();

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