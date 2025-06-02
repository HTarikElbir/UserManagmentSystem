namespace UserManagementSystem.Data.Entities;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<User> Users { get; set; } = new List<User>();    
}