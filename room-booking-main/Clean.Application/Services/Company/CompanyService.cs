using Clean.Application.Abstractions;
using Clean.Application.Dtos.Company;
using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Company;

public class CompanyService(IDbContext context):ICompanyService
{
    public async Task<PagedResponse<CompanyGetDto>> GetAll(CompanyFilter filter)
    {        var query = context.Comapanies.AsQueryable();
        var totalCount = await query.CountAsync();
        var companies =
            
            await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var dto = companies.Select(c => new CompanyGetDto()
        {
            Id = c.Id,
            Name = c.Name,
            PasswordHash = c.PasswordHash
        }).ToList();
        return new PagedResponse<CompanyGetDto>(
            data: dto,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            totalRecords: totalCount,
            message: "Companies Retrieved successfully! "
        );
    }

    public async Task<Response<CompanyGetDto>> GetById(int id)
    {
        var find = await context.Comapanies.FindAsync(id);
        if(find == null) return new Response<CompanyGetDto>(404,"No Company found!");
        var dto = new CompanyGetDto
        {
            Id = find.Id,
            Name = find.Name,
            PasswordHash = find.PasswordHash
        };
        return new Response<CompanyGetDto>(200,"Company found! ",dto);
    }

    public async Task<Response<CompanyGetDto>> Add(CompanyCreateDto dto)
    {
        var model = new Domain.Enitities.Company()
        {
            Name = dto.Name,
            PasswordHash = dto.PasswordHash
        };
        context.Comapanies.Add(model);
        await context.SaveChangesAsync();
        var company = new CompanyGetDto
        {
            Id = model.Id,
            Name = model.Name,
            PasswordHash = model.PasswordHash
        };
        return new Response<CompanyGetDto>(200, "Company added!", company);
    }

    public async Task<Response<string>> Update(CompanyCreateDto dto)
    {
        var find = await context.Comapanies.FindAsync(dto.Id);
        if(find == null) return new Response<string>(404,"No Company found!");
        find.Name = dto.Name;
        find.PasswordHash = dto.PasswordHash;
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