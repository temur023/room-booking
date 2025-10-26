namespace Clean.Application.Dtos.Authentication;

public class LoginRequestDto
{
    public string ComapanyName { get; set; }
    public string CompanyPassword { get; set; }
    public string FullName { get; set; }
    public string UserPassword { get; set; }
}