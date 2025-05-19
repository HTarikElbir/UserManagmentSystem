namespace UserManagementSystem.Business.Dtos.Auth;

public class LogoutDto
{
    public string Token { get; set; } = null!;
    public string Email { get; set; } = null!;
}