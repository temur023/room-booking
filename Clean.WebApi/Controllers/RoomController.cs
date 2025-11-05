using Clean.Application.Dtos.Room;
using Clean.Application.Filters;
using Clean.Application.Services.Room;
using Clean.Domain.Enitities;
using Clean.Permissions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController(IRoomService service):Controller
{
    [PermissionAuthorize(PermissionConstants.Rooms.View)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] RoomFilter filter)
    {
        return Ok(await service.GetAll(filter));
    }
    [PermissionAuthorize(PermissionConstants.Rooms.View)]
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.GetById(id));
    }
    [PermissionAuthorize(PermissionConstants.Rooms.View)]
    [HttpGet("get-available-rooms")]
    public async Task<IActionResult> GetAvailableRooms(DateOnly date, int startBlock, int endBlock)
    {
        return Ok(await service.GetAvailableRooms(date, startBlock, endBlock));
    }

    [PermissionAuthorize(PermissionConstants.Rooms.View)]
    [HttpGet("get-by-name")]
    public async Task<IActionResult> GetByName(string name,RoomFilter filter)
    {
        return Ok(await service.GetByName(name, filter));
    }
    [PermissionAuthorize(PermissionConstants.Rooms.Manage)]
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateRoomDto room)
    {
        
        return Ok(await service.Add(room));
    }
    [PermissionAuthorize(PermissionConstants.Rooms.Manage)]
    [HttpPut("update")]
    public async Task<IActionResult> Update(CreateRoomDto room)
    {
        
        return Ok(await service.Update(room));
    }

    [PermissionAuthorize(PermissionConstants.Rooms.Manage)]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await service.Delete(id));
    }
}