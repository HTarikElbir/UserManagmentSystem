using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Contexts;

// ApplicationDbContext is the EF Core database context for managing Users, Roles, and UserRole relationships.
// It configures entity relationships and provides DbSet properties for each entity.
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    // Table names
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Many-to-many relationship between User And Role
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        // Configure relationship: UserRole -> User (many-to-one)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        // Configure relationship: UserRole -> Role (many-to-one)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}