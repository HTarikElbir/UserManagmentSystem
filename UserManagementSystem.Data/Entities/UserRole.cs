namespace UserManagementSystem.Data.Entities;

// Represents the many-to-many relationship between users and roles in the system.
public class UserRole
{
    public int UserId { get; set; }  // Foreign key to User
    public int RoleId { get; set; }  // Foreign key to Role
}