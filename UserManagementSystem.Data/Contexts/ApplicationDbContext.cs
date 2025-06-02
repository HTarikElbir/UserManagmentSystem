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
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Primary Key
            entity.HasKey(u => u.UserId);
            
            entity.ToTable("Users");
            
            // Required fields and their configurations
            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Phone)
                .IsRequired()
                .HasMaxLength(20);

            // Unique Indexes
            entity.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email_CaseSensitive");

            entity.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_User_UserName_CaseSensitive");

            // Default value for IsActive
            entity.Property(u => u.IsActive)
                .HasDefaultValue(false);
        });
        
        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.RoleId);
    
            entity.ToTable("Roles");
            
            entity.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        
            entity.Property(r => r.Description)
                .HasMaxLength(200);
        
            entity.HasIndex(r => r.RoleName)
                .IsUnique()
                .HasDatabaseName("IX_Role_Name_CaseSensitive");
        
            entity.Property(r => r.IsActive)
                .HasDefaultValue(true);
        });
        
        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            // Composite Primary Key
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            // Table name ve schema
            entity.ToTable("UserRoles");
            
            entity.Property(ur => ur.UserId).IsRequired();
            entity.Property(ur => ur.RoleId).IsRequired();

            // Indexes
            entity.HasIndex(ur => ur.UserId);
            entity.HasIndex(ur => ur.RoleId);

            // Configure relationship: UserRole -> User (many-to-one)
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship: UserRole -> Role (many-to-one)
            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configure Department entity
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.DepartmentId);
            
            entity.ToTable("Departments");
            
            entity.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);
    
            entity.HasIndex(d => d.DepartmentName)
                .IsUnique()
                .HasDatabaseName("IX_Department_Name_CaseSensitive");
            
            entity.Property(d => d.IsActive)
                .HasDefaultValue(true);
    
            // Configure relationship: Department -> Users (one-to-many)
            entity.HasMany(d => d.Users)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); //  Users should not be deleted when a section is deleted
        });
        
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin", Description = "System administrator role. Has all permissions."},
            new Role { RoleId = 2, RoleName = "User", Description = "Regular user role" }
        );
        
        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department 
            { 
                DepartmentId = 1, 
                DepartmentName = "IT", 
                Description = "Information Technology Department",
                IsActive = true
            },
            new Department 
            { 
                DepartmentId = 2, 
                DepartmentName = "HR", 
                Description = "Human Resources Department",
                IsActive = true
            }
        );
        
        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                UserName = "aliveli",
                Password = BCrypt.Net.BCrypt.HashPassword("Aliveli123321."),
                Phone = "+905012345678",
                Email = "aliveli@example.com",
                DepartmentId = 1,
                IsActive = true
            },
            new User
            {
                UserId = 2,
                UserName = "aysefatma",
                Password = BCrypt.Net.BCrypt.HashPassword("Aysefatma123321."),
                Phone = "+905112345678",
                Email = "aysefatma@example.com",
                DepartmentId = 2,
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