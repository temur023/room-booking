using Clean.Application.Dtos.RoomEquipment;
using Clean.Application.Services.RoomEquipment;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomEquipmentController(IRoomEquipmentService service):Controller
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAll());
    }
    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.GetById(id));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(RoomEquipmentCreateDto equipment)
    {
        return Ok(await service.Add(equipment));
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(RoomEquipmentCreateDto equipment)
    {
        return Ok(await service.Update(equipment));
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        return Ok(await service.Delete(id));
    }
}