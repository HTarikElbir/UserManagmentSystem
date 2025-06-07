namespace UserManagementSystem.Business.Dtos.Department;

public class DepartmentUpdateDto
{
    public string DepartmentName { get; set; } = null!;
    public string DepartmentDescription { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}