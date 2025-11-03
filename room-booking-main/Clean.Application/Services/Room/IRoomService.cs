using Clean.Application.Dtos.Room;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clean.Application.Services.Room;

public interface IRoomService
{
    Task<PagedResponse<GetRoomDto>> GetAll(RoomFilter filter);
    Task<Response<GetRoomDto>> GetById(int id);
    Task<Response<GetRoomDto>> Add(CreateRoomDto dto);
    Task<Response<string>> Update(CreateRoomDto dto);
    Task<Response<string>> Delete(int id);
}