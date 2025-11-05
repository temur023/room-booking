using System.Security.Claims;
using Clean.Application.Abstractions;
using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Enitities;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Authentication;

public class UserService(IDbContext context, IHttpContextAccessor httpContextAccessor):IUserService
{
    public async Task<PagedResponse<UserGetDto>> GetAll(UserFilter filter,int? companyId)
    {
        var query = context.Users.Where(u=>u.CompanyId == companyId).AsQueryable();
        var totalCount = await query.CountAsync();
        var users = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var dto = users.Select(user => new UserGetDto()
        {
            Id = user.Id,
            CompanyId = user.CompanyId,
            FullName = user.FullName,
            UserName = user.UserName,
            Role = user.Role
        }).ToList();
        return new PagedResponse<UserGetDto>(
            data: dto,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            totalRecords: totalCount,
            message: "Users Retrieved successfully! "
        );
    }

    public async Task<Response<UserGetDto>> GetById(int id, int? companyId)
    {
        var find = await context.Users.FirstOrDefaultAsync(u=>u.Id == id && u.CompanyId == companyId);
        if(find == null) return new Response<UserGetDto>(404,"User not found");
        var dto = new UserGetDto
        {
            Id = id,
            CompanyId = find.CompanyId,
            FullName = find.FullName,
            UserName = find.UserName,
            Role = find.Role
        };
        return new Response<UserGetDto>(200, "User successfully found", dto);
    }

    public async Task<Response<UserGetDto>> Add(UserCreateDto dto, int companyId)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var model = new User()
        {
            FullName = dto.FullName,
            CompanyId = companyId,
            UserName = dto.UserName,
            PasswordHash = hashedPassword,
            Role = dto.Role
        };
        context.Users.Add(model);
        await context.SaveChangesAsync();
        var userdto = new UserGetDto
        {
            Id = model.Id,
            CompanyId = companyId,
            UserName = model.UserName,
            FullName = model.FullName,
            Role = model.Role
        };
        return new Response<UserGetDto>(200, "User successfully added!", userdto);
    }

    public async Task<Response<string>> Update(UserCreateDto dto, int companyId)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var find = await context.Users.FirstOrDefaultAsync(u=>u.Id == dto.Id && u.CompanyId == companyId);
        if(find == null) return new Response<string>(404,"User not found");
        find.FullName = dto.FullName;
        find.UserName = dto.UserName;
        find.PasswordHash = hashedPassword;
        find.Role = dto.Role;
        find.CompanyId = dto.CompanyId;
        await context.SaveChangesAsync();
        return new Response<string>(200, "User successfully updated");
    }

    public async Task<Response<string>> Delete(int id, int companyId)
    {
        var find = await context.Users.FirstOrDefaultAsync(u=>u.Id == id && u.CompanyId == companyId);
        if(find == null) return new Response<string>(404,"User not found");
        context.Users.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200,"User successfully deleted!");
    }
    
    //For user
    public async Task<Response<string>> UpdateForUser(UserUpdateDto dto)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier );
        if(userIdClaim == null) return new Response<string>(401,"Unauthorized");
        
        var userId =  int.Parse(userIdClaim.Value);
        
        var user = await context.Users.FindAsync(userId);
        if(user == null) return new Response<string>(404,"User not found");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
        if(!isPasswordValid) return new Response<string>(400,"Invalid password");
        
        user.FullName = dto.FullName ?? user.FullName;
        user.UserName = dto.UserName ?? user.UserName;
        if(!string.IsNullOrEmpty(dto.NewPassword))
            user.PasswordHash =  BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        
        await context.SaveChangesAsync();
        return new Response<string>(200,"Your data have been updated!");
    }

    public async Task<Response<string>> DeleteForUser(string currentPassword)
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier );
        if(userIdClaim == null) return new Response<string>(401,"Unauthorized");
        
        var userId =  int.Parse(userIdClaim.Value);
        
        var user = await context.Users.FindAsync(userId);
        if(user == null) return new Response<string>(404,"User not found");
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash);
        if(!isPasswordValid) return new Response<string>(400,"Invalid password");
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return new Response<string>(200,"Your data have been deleted!");
    }
}