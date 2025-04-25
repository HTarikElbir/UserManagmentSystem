namespace UserManagementSystem.Business.Dtos;

public class UserUpdateDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
}