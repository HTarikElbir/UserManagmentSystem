using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Data.Entities;

/// <summary>
/// Represents a user in the system, containing basic user information
/// such as username, password, email, phone, department, and status.
/// This class also manages the many-to-many relationship with roles through the UserRole entity.
/// </summary>
public class User
{
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "UserName is required")]
    public string UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
    
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;
    
    [Phone]
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Department is required")]
    public string Department { get; set; } = string.Empty;

    public bool IsActive { get; set; } = false;
    
    // Relationship with UserRole (many-to-many)
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}