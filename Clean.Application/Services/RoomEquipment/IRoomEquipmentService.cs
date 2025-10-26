using Clean.Application.Dtos.RoomEquipment;
using Clean.Application.Responses;

namespace Clean.Application.Services.RoomEquipment;

public interface IRoomEquipmentService
{
    Task<Response<List<RoomEquipmentGetDto>>> GetAll();
    Task<Response<RoomEquipmentGetDto>> GetById(int id);
    Task<Response<List<RoomEquipmentGetDto>>> GetByName(string name);
    Task<Response<RoomEquipmentGetDto>> Add(RoomEquipmentCreateDto dto);
    Task<Response<string>> Update(RoomEquipmentCreateDto dto);
    Task<Response<string>> Delete(int id);
}