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
    
    public async Task<bool> ResetPasswordAsync(int userId, string password)
    {
        var user = await _context.Users.FindAsync(userId);
        
        user!.Password = password;
        await _context.SaveChangesAsync();
        
        return true;
    }
}