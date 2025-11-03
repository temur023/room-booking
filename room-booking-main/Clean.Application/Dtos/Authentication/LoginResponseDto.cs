namespace Clean.Application.Dtos.Authentication;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string UserName { get; set; }
    public string Role { get; set; } = null!;
}