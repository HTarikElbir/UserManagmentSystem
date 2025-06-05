namespace UserManagementSystem.Business.Dtos.Department;

public class DepartmentAddDto
{
    public string DepartmentName { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}