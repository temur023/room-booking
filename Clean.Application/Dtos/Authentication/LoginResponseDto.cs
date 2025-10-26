namespace Clean.Application.Dtos.Authentication;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string Role { get; set; } = null!;
}