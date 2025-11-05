using Clean.Application.Dtos.Authentication;
using Clean.Application.Responses;

namespace Clean.Application.Services.Authentication;

public interface IAuthService
{
    Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
}