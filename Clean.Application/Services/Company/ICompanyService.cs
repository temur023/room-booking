using Clean.Application.Dtos.Company;
using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;

namespace Clean.Application.Services.Company;

public interface ICompanyService
{
    Task<PagedResponse<CompanyGetDto>> GetAll(CompanyFilter filter);
    Task<Response<CompanyGetDto>> GetById(int id);
    Task<Response<List<CompanyOverviewDto>>> GetCompanyOverview();
    Task<Response<List<RoomUtilizationDto>>> GetRoomUtilization(int id);
    Task<Response<CompanyGetDto>> Add(CompanyCreateDto dto);
    Task<Response<string>> Update(CompanyCreateDto dto);
    Task<Response<string>> Delete(int id);
}
