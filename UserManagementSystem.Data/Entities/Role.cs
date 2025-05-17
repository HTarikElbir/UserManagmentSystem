using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Data.Entities;

// Represents a role within the application (e.g., Admin, User, Moderator).
// Each user can be assigned one or more roles to control access and permissions.
public class Role
{
    public int RoleId { get; set; }
    
    public string RoleName { get; set; } = null!;
    
    public string Description { get; set; } = string.Empty;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}










// public string NormalizedRoleName { get; set; } = null!;
// public string RoleDescription { get; set; } = null!;
// public DateTime CreatedDate { get; set; }
// public bool IsActive { get; set; }