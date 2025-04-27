namespace UserManagementSystem.Business.Dtos;

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    
    public string Phone { get; set; } = null!;
    public string Department { get; set; } = null!;
    public List<RoleDto> Roles { get; set; } = null!;
}