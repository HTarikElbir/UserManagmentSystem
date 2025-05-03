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
    
    // Returns a role by id
    public async Task<Role?> GetRoleByIdAsync(int roleId) => await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);

    // Returns a role by name
    public async Task<Role?> GetRoleByNameAsync(string roleName) => await  _context.Roles
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
    
    public async Task AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role); 
        await _context.SaveChangesAsync();
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