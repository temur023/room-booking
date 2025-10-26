using Clean.Domain.Enitities;

namespace Clean.Application.Services.Authentication;

public interface ITokenService
{
    Task<string> GenerateJwtToken(User user);
}