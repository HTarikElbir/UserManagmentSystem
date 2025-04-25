using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserManagementSystem.Data.Entities;

/// <summary>
/// Represents a user in the system, containing basic user information
/// such as username, password, email, phone, department, and status.
/// This class also manages the many-to-many relationship with roles through the UserRole entity.
/// </summary>
public class User
{
    public int UserId { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Department { get; set; } = string.Empty;

    public bool IsActive { get; set; } = false;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}