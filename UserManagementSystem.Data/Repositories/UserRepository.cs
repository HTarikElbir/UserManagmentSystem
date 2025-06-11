using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Extensions;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

// Repository class for managing User entity operations in the database.
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Returns a list of all users in the system.
    public async Task<List<User>> GetAllUsersAsync(int page, int pageSize) => await _context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .AsNoTracking()
        .Paginate(page,pageSize)
        .ToListAsync();

    public async Task<List<User>> GetAllUsersWithoutPaginationAsync()
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Department)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByDepartmentWithoutPaginationAsync(int departmentId)
    {
        return await _context.Users
            .Where(u => u.DepartmentId == departmentId)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Department)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByRoleWithoutPaginationAsync(int roleId)
    {
        return await _context.Users
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Department)
            .AsNoTracking()
            .ToListAsync();
    }


    // Returns a user by their unique ID.
    public async Task<User?> GetByIdUserAsync(int userId) => await _context.Users
        .Include(u=>u.UserRoles)
            .ThenInclude(ur=>ur.Role)
        .FirstOrDefaultAsync(u => u.UserId == userId);
    
    // Returns a user by their email address.
    public async  Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    // Adds a new user to the database. 
    public async Task AddUserAsync(User user)
    { 
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    // Updates an existing user's data.
    public async Task UpdateUserAsync(User user)
    {   
        _context.Users.Update(user); 
        await _context.SaveChangesAsync();
    }
    
    // Deletes a user by ID if found.
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        
        _context.Users.Remove(user!);
        
        await _context.SaveChangesAsync();
        
    }
    
    public async Task<List<User>> GetUsersByDepartmentAsync(int departmentId,int page, int pageSize)
    {
        // // Returns a list of users that belong to the specified department.
        return await _context.Users
            .Where(u => u.DepartmentId == departmentId)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .Paginate(page,pageSize)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetUsersByRoleAsync(string roleName,int page, int pageSize)
    {
        // Returns a list of users who have the specified role.
        // It checks if the role name associated with each user's UserRoles matches the given role name.
        return await _context.Users
            .Where(u => u.UserRoles.Any(ur => ur.Role.RoleName.ToLower() == roleName.ToLower()))
            .AsNoTracking()
            .Paginate(page,pageSize)
            .ToListAsync();
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
}