using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly ApplicationDbContext _context;
        
    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // This method checks if the user exists in the database and verifies the password.
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
}