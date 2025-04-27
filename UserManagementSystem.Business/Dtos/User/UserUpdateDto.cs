namespace UserManagementSystem.Business.Dtos.User;

public class UserUpdateDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
}