using Clean.Application.Abstractions;
using Clean.Application.Dtos.RoomEquipment;
using Clean.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.RoomEquipment;

//We add update remove all equipments of a room through this service
public class RoomEquipmentService(IDbContext context):IRoomEquipmentService
{
    
    public async Task<Response<List<RoomEquipmentGetDto>>> GetAll()
    {
        var equiments = context.RoomEquipments.Select(e => new RoomEquipmentGetDto()
        {
            Id = e.Id,
            Name = e.Name,
            RoomId = e.RoomId
        }).ToList();
        return new Response<List<RoomEquipmentGetDto>>(200,"equipments retrieved successfully: ", equiments);
    }

    public async Task<Response<RoomEquipmentGetDto>> GetById(int id)
    {
        var find = await context.RoomEquipments.FindAsync(id);
        if (find == null) return new Response<RoomEquipmentGetDto>(404, "No Equipments Found!");
        var equipment = new RoomEquipmentGetDto()
        {
            Id = find.Id,
            Name = find.Name,
            RoomId = find.RoomId
        };
        return new Response<RoomEquipmentGetDto>(200, "equipment retrieved successfully: ", equipment);
    }

    public async Task<Response<List<RoomEquipmentGetDto>>> GetByName(string name)
    {
        var find = context.RoomEquipments.Where(e=>e.Name.ToLower().Contains(name.ToLower()));
        if(find == null) return new Response<List<RoomEquipmentGetDto>>(404, "No Equipments Found!");
        var equipments = await find.Select(e => new RoomEquipmentGetDto()
        {
            Id = e.Id,
            Name = e.Name,
            RoomId = e.RoomId
        }).ToListAsync();
        return new Response<List<RoomEquipmentGetDto>>(200, "equipments retrieved successfully: ", equipments);
    }

    public async Task<Response<RoomEquipmentGetDto>> Add(RoomEquipmentCreateDto dto)
    {
        var model = new Domain.Enitities.RoomEquipment()
        {
            RoomId = dto.RoomId,
            Name = dto.Name,
        };
        context.RoomEquipments.Add(model);
        await context.SaveChangesAsync();

        var equipment = new RoomEquipmentGetDto()
        {
            Id = model.Id,
            Name = model.Name,
            RoomId = model.RoomId
        };
        return new Response<RoomEquipmentGetDto>(200, "equipment added successfully", equipment);
    }

    public async Task<Response<string>> Update(RoomEquipmentCreateDto dto)
    {
        var find = await context.RoomEquipments.FindAsync(dto.Id);
        if (find == null) return new Response<string>(404, "No Equipments Found!");
        find.Name = dto.Name;
        find.RoomId = dto.RoomId;
        await context.SaveChangesAsync();
        return new Response<string>(200, "equipment updated successfully!");
    }

    public async Task<Response<string>> Delete(int id)
    {
        var find = await context.RoomEquipments.FindAsync(id);
        if (find == null) return new Response<string>(404, "No Equipments Found!");
        context.RoomEquipments.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "equipment deleted successfully!");
    }
}