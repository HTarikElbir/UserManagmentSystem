namespace UserManagementSystem.Business.Dtos.User;

public class UserAddDto
{
    public string UserName { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string Phone { get; set; } = null!;
    
    public string Department { get; set; } = null!;
    
}