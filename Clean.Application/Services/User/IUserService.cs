using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Enitities;

namespace Clean.Application.Services.Authentication;

public interface IUserService
{
    Task<PagedResponse<UserGetDto>> GetAll(UserFilter filter, int? companyId);
    Task<Response<UserGetDto>> GetById(int id, int? companyId);
    Task<Response<UserGetDto>> Add(UserCreateDto dto, int companyId);
    Task<Response<string>> Update(UserCreateDto dto, int companyId);
    Task<Response<string>> Delete(int id, int companyId);
    
    Task<Response<string>> UpdateForUser(UserUpdateDto dto);
    Task<Response<string>> DeleteForUser(string currentPassword);
}