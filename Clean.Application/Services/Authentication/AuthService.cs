using Clean.Application.Abstractions;
using Clean.Application.Dtos.Authentication;
using Clean.Application.Responses;
using Clean.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Authentication;

public class AuthService(IDbContext context, ITokenService tokenService,IPasswordHasher<Comapany> companyHasher, IPasswordHasher<User> userHasher):IAuthService
{
    public async Task<Response<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var company = await context.Comapanies.Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Name == dto.ComapanyName);
        if (company == null)
        {
            return new Response<LoginResponseDto>(404,"company not found");
        }
        var companyVerify = companyHasher.VerifyHashedPassword(company, company.PasswordHash, dto.ComapanyName);
        if(companyVerify == PasswordVerificationResult.Failed) return new Response<LoginResponseDto>(401, "invalid password!");

        var user = company.Users.FirstOrDefault(u => u.FullName == dto.FullName);
        if(user == null) return new Response<LoginResponseDto>(404, "user not found!");
        
        var userVerify = userHasher.VerifyHashedPassword(user, user.PasswordHash, dto.ComapanyName);
        if (userVerify == PasswordVerificationResult.Failed) return new Response<LoginResponseDto>(401, "invalid password!");

        var token = await tokenService.GenerateJwtToken(user);

        var response = new LoginResponseDto()
        {
            Token = token,
            FullName = user.FullName,
            CompanyName = company.Name,
            Role = user.Role
        };
        
        return new Response<LoginResponseDto>(200,"Login Successful",response);
    }
}