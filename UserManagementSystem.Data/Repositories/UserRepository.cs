using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
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
    public async Task<List<User>> GetAllUsers() => await _context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .ToListAsync();
    
    
    // Returns a user by their unique ID.
    public async Task<User?> GetByIdUser(int userId) => await _context.Users
        .Include(u=>u.UserRoles)
            .ThenInclude(ur=>ur.Role)
        .FirstOrDefaultAsync(u => u.UserId == userId);
    
    // Returns a user by their email address.
    public async Task<User?> GetByEmail(string email) => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
    // Adds a new user to the database. 
    public async Task AddUserAsync(User user)
    { 
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    
    // Updates an existing user's data.
    public async  Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user); 
        await _context.SaveChangesAsync();
    }
    
    // Deletes a user by ID if found.
    public async  Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
    
    public Task<List<User>> GetUsersByDepartmentAsync(string department)
    {
        // Returns a list of users that belong to the specified department.
        // Uses CurrentCultureIgnoreCase for case-insensitive comparison based on the current culture.
       return _context.Users
           .Where(u => string.Equals(u.Department, department, StringComparison.CurrentCultureIgnoreCase))
           .ToListAsync();
    }
    
    public Task<List<User>> GetUsersByRoleAsync(string roleName)
    {
        // Returns a list of users who have the specified role.
        // It checks if the role name associated with each user's UserRoles matches the given role name.
        return _context.Users
            .Where(u => u.UserRoles.Any(ur => ur.Role.RoleName == roleName))
            .ToListAsync();
    }
}