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
        
        // // Configure User entity
        // modelBuilder.Entity<User>()
        //     .HasKey(u => u.UserId);
        
        // Configure User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        // Many-to-many relationship between User And Role
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        
        // Configure relationship: UserRole -> User (many-to-one)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure relationship: UserRole -> Role (many-to-one)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin" },
            new Role { RoleId = 2, RoleName = "User" }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                UserName = "aliveli",
                Password = "Ali123!!!",
                Phone = "+905012345678",
                Email = "aliveli@example.com",
                Department = "IT",
                IsActive = true
            },
            new User
            {
                UserId = 2,
                UserName = "aysefatma",
                Password = "Ayse123!!!",
                Phone = "+905112345678",
                Email = "aysefatma@example.com",
                Department = "HR",
                IsActive = true
            }
        );

        // Seed UserRoles
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = 1, RoleId = 1 },
            new UserRole { UserId = 2, RoleId = 2 }
        );
    }
}