using Clean.Application.Dtos.RoomEquipment;
using Clean.Application.Services.RoomEquipment;
using Clean.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomEquipmentController(IRoomEquipmentService service):Controller
{
    [PermissionAuthorize(PermissionConstants.RoomEquipments.View)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAll());
    }
    [PermissionAuthorize(PermissionConstants.RoomEquipments.View)]
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.GetById(id));
    }
    [PermissionAuthorize(PermissionConstants.RoomEquipments.Manage)]
    [HttpPost("add")]
    public async Task<IActionResult> Add(RoomEquipmentCreateDto equipment)
    {
        return Ok(await service.Add(equipment));
    }
    [PermissionAuthorize(PermissionConstants.RoomEquipments.Manage)]
    [HttpPut("update")]
    public async Task<IActionResult> Update(RoomEquipmentCreateDto equipment)
    {
        return Ok(await service.Update(equipment));
    }
    [PermissionAuthorize(PermissionConstants.RoomEquipments.Manage)]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await service.Delete(id));
    }
}