using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Services.Authentication;
using Clean.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController(IUserService service):Controller
{
    [PermissionAuthorize(PermissionConstants.Users.View)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] UserFilter filter)
    {
        var companyIdClaim = User.FindFirst("companyId")?.Value;
        if (companyIdClaim == null)
            return Unauthorized("Missing CompanyId in token");
        int companyId = int.Parse(companyIdClaim); 
        
        return Ok(await service.GetAll(filter, companyId));
    }
    [PermissionAuthorize(PermissionConstants.Users.View)]
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        var companyIdClaim = User.FindFirst("companyId")?.Value;
        if (companyIdClaim == null)
            return Unauthorized("Missing CompanyId in token");
        int companyId = int.Parse(companyIdClaim); 
        return Ok(await service.GetById(id,  companyId));
    }
    [PermissionAuthorize(PermissionConstants.Users.Manage)]
    [HttpPost("add")]
    public async Task<IActionResult> Add(UserCreateDto model)
    {
        var companyIdClaim = User.FindFirst("companyId")?.Value;
        if (companyIdClaim == null)
            return Unauthorized("Missing CompanyId in token");
        int companyId = int.Parse(companyIdClaim); 
        return Ok(await service.Add(model, companyId));
    }
    [PermissionAuthorize(PermissionConstants.Users.Manage)]
    [HttpPut("update")]
    public async Task<IActionResult> Update(UserCreateDto model)
    {
        var companyIdClaim = User.FindFirst("companyId")?.Value;
        if (companyIdClaim == null)
            return Unauthorized("Missing CompanyId in token");
        int companyId = int.Parse(companyIdClaim); 
        return Ok(await service.Update(model, companyId));
    }
    [PermissionAuthorize(PermissionConstants.Users.Manage)]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var companyIdClaim = User.FindFirst("companyId")?.Value;
        if (companyIdClaim == null)
            return Unauthorized("Missing CompanyId in token");
        int companyId = int.Parse(companyIdClaim); 
        
        return Ok(await service.Delete(id, companyId));
    }

    [PermissionAuthorize(PermissionConstants.Users.ManageSelf)]
    [HttpPut("update-self")]
    public async Task<IActionResult> UpdateSelf(UserUpdateDto model)
    {
        return Ok(await service.UpdateForUser(model));
    }
    [PermissionAuthorize(PermissionConstants.Users.ManageSelf)]
    [HttpDelete("delete-self")]
    public async Task<IActionResult> DeleteSelf(string currentPassword)
    {
        return Ok(await service.DeleteForUser(currentPassword));
    }
}