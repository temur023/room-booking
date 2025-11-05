using Clean.Application.Dtos.Company;
using Clean.Application.Filters;
using Clean.Application.Services.Company;
using Clean.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController(ICompanyService service):Controller
{
    [PermissionAuthorize(PermissionConstants.Company.View)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] CompanyFilter filter)
    {
        return Ok(await service.GetAll(filter));
    }
    [PermissionAuthorize(PermissionConstants.Company.View)]
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.GetById(id));
    }

    [PermissionAuthorize(PermissionConstants.Company.ViewOverview)]
    [HttpGet("get-company-overview")]
    public async Task<IActionResult> GetOverview()
    {
        return Ok(await service.GetCompanyOverview());
    }
    [PermissionAuthorize(PermissionConstants.Rooms.Manage)]
    [HttpGet("room-utilization")]
    public async Task<IActionResult> GetRoomUtilization(int id)
    {
        return Ok(await service.GetRoomUtilization(id));
    }
    [PermissionAuthorize(PermissionConstants.Company.Manage)]
    [HttpPost("add")]
    public async Task<IActionResult> Add(CompanyCreateDto model)
    {
        return Ok(await service.Add(model));
    }
    [PermissionAuthorize(PermissionConstants.Company.Manage)]
    [HttpPut("update")]
    public async Task<IActionResult> Update(CompanyCreateDto model)
    {
        return Ok(await service.Update(model));
    }
    [PermissionAuthorize(PermissionConstants.Company.Manage)]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await service.Delete(id));
    }
}