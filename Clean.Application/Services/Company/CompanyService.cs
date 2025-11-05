using System.Security.Claims;
using Clean.Application.Abstractions;
using Clean.Application.Dtos.Company;
using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Enitities;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Company;

public class CompanyService(IDbContext context,IHttpContextAccessor httpContextAccessor):ICompanyService
{
    private Role? GetCurrentRole() =>
        Enum.TryParse<Role>(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value, out var role) ? role : null;

    private int? GetCompanyId() =>
        int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst("companyId")?.Value, out var companyId) ? companyId : null;

    private bool IsSuperAdmin => GetCurrentRole() == Role.SuperAdmin;
    public async Task<PagedResponse<CompanyGetDto>> GetAll(CompanyFilter filter)
    {        var query = context.Comapanies.AsQueryable();
        var totalCount = await query.CountAsync();
        var companies =
            
            await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var dto = companies.Select(c => new CompanyGetDto()
        {
            Id = c.Id,
            Name = c.Name,
        }).ToList();
        return new PagedResponse<CompanyGetDto>(
            data: dto,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            totalRecords: totalCount,
            message: "Companies Retrieved successfully! "
        );
    }

    public async Task<Response<List<CompanyOverviewDto>>> GetCompanyOverview()
    {
        var currentCompany = GetCompanyId();
        var currentRole = GetCurrentRole();
        var query = context.Comapanies
            .Include(c => c.Rooms)
            .Include(c => c.Users)
            .AsQueryable();
        if (!IsSuperAdmin)
        {
            query = query.Where(c=>c.Id ==  currentCompany);
        }
        
        var dto = await query.Select(c=> new CompanyOverviewDto
        {
            Id = c.Id,
            Name = c.Name,
            TotalRooms = c.Rooms.Count,
            TotalUsers = c.Users.Count
        }).ToListAsync();
        return new Response<List<CompanyOverviewDto>>(200, "Company Overview: ", dto);
    }
    public async Task<Response<List<RoomUtilizationDto>>> GetRoomUtilization(int id)
    {
        var companyId = GetCompanyId();
        var query = context.Comapanies
            .Include(c => c.Rooms)
            .ThenInclude(r => r.Bookings)
            .AsQueryable();

        if (!IsSuperAdmin)
        {
            query = query.Where(c => c.Id == companyId);
        }
        
        const double totalBlocks = 8 * 5;
        
        var dto = await query
            .SelectMany(c => c.Rooms)
            .Where(r => r.Id == id) // Get specific room by ID
            .Select(r => new RoomUtilizationDto
            {
                RoomId = r.Id,
                RoomName = r.RoomName,
                UtilizationPercent = Math.Round(
                    (r.Bookings
                        .Where(b => b.Date >= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)))
                        .Sum(b => (b.EndBlock - b.StartBlock)) / totalBlocks) * 100,
                    2)
            })
            .ToListAsync();

        return new Response<List<RoomUtilizationDto>>(200, "Room utilization data retrieved successfully", dto);
    }
    
    public async Task<Response<CompanyGetDto>> GetById(int id)
    {
        var find = await context.Comapanies.FindAsync(id);
        if(find == null) return new Response<CompanyGetDto>(404,"No Company found!");
        var dto = new CompanyGetDto
        {
            Id = find.Id,
            Name = find.Name,
        };
        return new Response<CompanyGetDto>(200,"Company found! ",dto);
    }

    public async Task<Response<CompanyGetDto>> Add(CompanyCreateDto dto)
    {
        var model = new Domain.Enitities.Company()
        {
            Name = dto.Name,
        };
        context.Comapanies.Add(model);
        await context.SaveChangesAsync();
        var company = new CompanyGetDto
        {
            Id = model.Id,
            Name = model.Name,
        };
        return new Response<CompanyGetDto>(200, "Company added!", company);
    }

    public async Task<Response<string>> Update(CompanyCreateDto dto)
    {
        var find = await context.Comapanies.FindAsync(dto.Id);
        if(find == null) return new Response<string>(404,"No Company found!");
        find.Name = dto.Name;
        context.Comapanies.Update(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Company updated!");
    }

    public async Task<Response<string>> Delete(int id)
    {
        var  find = await context.Comapanies.FindAsync(id);
        if(find == null) return new Response<string>(404,"No Company found!");
        context.Comapanies.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Company deleted!");
    }
}