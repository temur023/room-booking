using Clean.Application.Abstractions;
using Clean.Application.Dtos.Authentication;
using Clean.Application.Responses;
using Clean.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Authentication;

public class AuthService(IDbContext context, ITokenService tokenService):IAuthService
{
    public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u=>u.UserName == dto.UserName);
       
        if(user == null) return new Response<LoginResponseDto>(404, "user not found!");
        
        if (!BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.PasswordHash))
            return new Response<LoginResponseDto>(401, "Invalid user password!");

        var token = await tokenService.GenerateJwtToken(user);

        var response = new LoginResponseDto()
        {
            Token = token,
            UserName = user.UserName,
            Role = user.Role.ToString()
        };
        
        return new Response<LoginResponseDto>(200,"Login Successful",response);
    }
}