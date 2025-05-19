using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Extensions;
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
    public async Task<List<Role>> GetAllRolesAsync(int page = 1, int pageSize = 10) => await  _context.Roles
        .AsNoTracking()
        .Paginate(page,pageSize)
        .ToListAsync();
    
    // Returns a role by id
    public async Task<Role?> GetRoleByIdAsync(int roleId) => await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);

    // Returns a role by name
    public async Task<Role?> GetRoleByNameAsync(string roleName) => await  _context.Roles
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
    
    // Adds a new role to the database. 
    public async Task AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role); 
        await _context.SaveChangesAsync();
    }
    
    // Updates an existing role's data.
    public async Task UpdateRoleAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    // Deletes a role by ID if found.
    public async Task DeleteRoleAsync(int roleId)
    {
        var role = await _context.Roles.FindAsync(roleId);
        
        _context.Roles.Remove(role!); 
        await _context.SaveChangesAsync();
    }
}