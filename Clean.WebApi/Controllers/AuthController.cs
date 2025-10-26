using Clean.Application.Dtos.Authentication;
using Clean.Application.Services.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService service): Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Lgoin([FromBody] LoginRequestDto dto)
    {
        return Ok(await service.LoginAsync(dto));
    }
    
}