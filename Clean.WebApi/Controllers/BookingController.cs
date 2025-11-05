using Clean.Application.Dtos.Booking;
using Clean.Application.Filters;
using Clean.Application.Services.Booking;
using Clean.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace RoomBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController(IBookingService service):Controller
{
    [HttpGet("get-all")]
    [PermissionAuthorize(PermissionConstants.Bookings.View)]
    public async Task<IActionResult> GetAll([FromQuery] BookingFilter filter)
    {
        return Ok(await service.GetAll(filter));
    }
    [HttpPost("add")]
    [PermissionAuthorize(PermissionConstants.Bookings.Manage)]
    public async Task<IActionResult> Add(BookingCreateDto dto)
    {
        return Ok(await service.Add(dto));
    }
    [HttpPut("update")]
    [PermissionAuthorize(PermissionConstants.Bookings.Manage)]
    public async Task<IActionResult> Update(BookingCreateDto dto)
    {
        return Ok(await service.Update(dto));
    }
    [HttpDelete("delete")]
    [PermissionAuthorize(PermissionConstants.Bookings.Manage)]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await service.Delete(id));
    }

    [HttpGet("get-all-for-user")]
    [PermissionAuthorize(PermissionConstants.Bookings.ManageSelf)]
    public async Task<IActionResult> GetAllForUser([FromQuery] BookingFilter filter)
    {
        return Ok(await service.GetMyBookingsForUser(filter));
    }

    [HttpPost("add-for-user")]
    [PermissionAuthorize(PermissionConstants.Bookings.ManageSelf)]
    public async Task<IActionResult> AddForUser(BookingCreateDto dto)
    {
        return Ok(await service.AddForUser(dto));
    }

    [HttpPut("update-for-user")]
    [PermissionAuthorize(PermissionConstants.Bookings.ManageSelf)]
    public async Task<IActionResult> UpdateForUser(BookingCreateDto dto)
    {
        return Ok(await service.UpdateForUser(dto));
    }


    [HttpGet("get-most-used-rooms")]
    [PermissionAuthorize(PermissionConstants.Bookings.View)]
    public async Task<IActionResult> GetMostUsedRooms()
    {
        return Ok(await service.GetMostUsedRooms());
    }

    [HttpGet("get-user-activity")]
    [PermissionAuthorize(PermissionConstants.Bookings.Manage)]
    public async Task<IActionResult> GetUserActivity()
    {
        return Ok(await service.GetUserActivity());
    }

    [HttpGet("get-booking-trends")]
    [PermissionAuthorize(PermissionConstants.Bookings.View)]
    public async Task<IActionResult> GetBookingTrends()
    {
        return Ok(await service.GetBookingTrends());
    }
    
    [HttpDelete("delete-for-user")]
    [PermissionAuthorize(PermissionConstants.Bookings.ManageSelf)]
    public async Task<IActionResult> DeleteForUser(int id)
    {
        return Ok(await service.DeleteForUser(id));
    }
}