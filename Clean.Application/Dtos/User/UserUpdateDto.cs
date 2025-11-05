namespace Clean.Application.Dtos.User;

public class UserUpdateDto
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string CurrentPassword { get; set; }  
    public string? NewPassword { get; set; }  
}