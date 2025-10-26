using Clean.Application.Dtos.Room;
using Clean.Application.Filters;
using Clean.Application.Services.Room;
using Clean.Domain.Enitities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController(IRoomService service):Controller
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll([FromQuery] RoomFilter filter)
    {
        return Ok(await service.GetAll(filter));
    }
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.GetById(id));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(CreateRoomDto room)
    {
        return Ok(await service.Add(room));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(CreateRoomDto room)
    {
        return Ok(await service.Update(room));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await service.Delete(id));
    }
}