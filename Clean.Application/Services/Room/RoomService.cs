using Clean.Application.Abstractions;
using Clean.Application.Dtos.Room;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Room;

public class RoomService(IDbContext context):IRoomService
{
    public async Task<Response<List<GetRoomDto>>> GetAll(RoomFilter filter)
    {
        var query = context.Rooms.Include(r => r.Equipments).AsQueryable();
        if (filter.MaxCapacity.HasValue)
        {
            query = query.Where(q=>q.Capacity <= filter.MaxCapacity);
        }

        if (filter.MinCapacity.HasValue)
        {
            query = query.Where(q => q.Capacity >= filter.MinCapacity);
        }

        if (filter.Floor.HasValue)
        {
            query = query.Where(q=>q.Floor == filter.Floor);
        }
        
        if (filter.IsActive.HasValue)
        {
            query = query.Where(q => q.IsActive == filter.IsActive);
        }

        if (filter.Equipments?.Any() == true) 
        {
            query = query.Where(r => r.Equipments
                .Any(e => filter.Equipments
                    .Any(eq => e.Name.ToLower() == eq.ToLower())));
        }

        var totalCount = await query.CountAsync();
        var rooms = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var roomDto = rooms.Select(r => new GetRoomDto()
        {
            Id = r.Id,
            Capacity = r.Capacity,
            Floor = r.Floor,
            RoomName = r.RoomName,
            IsActive = r.IsActive,
            Equipments = r.Equipments,
        }).ToList();
        return new PagedResponse<GetRoomDto>(
            data: roomDto,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            totalRecords: totalCount,
            message:"Rooms retrieved successfully!");
    }

    public async Task<Response<GetRoomDto>> GetById(int id)
    {
        var find = await context.Rooms.Include(r=>r.Equipments).FirstOrDefaultAsync(r=>r.Id == id);
        if (find == null) return new Response<GetRoomDto>(404, "Room not found");
        var roomDto = new GetRoomDto()
        {
            Id = find.Id,
            Capacity = find.Capacity,
            Floor = find.Floor,
            RoomName = find.RoomName,
            IsActive = find.IsActive,
            Equipments = find.Equipments,
        };
        return new Response<GetRoomDto>(200, "Room found: ", roomDto);
    }

    public async Task<Response<GetRoomDto>> Add(CreateRoomDto dto)
    {
        var model = new Domain.Enitities.Room()
        {
            Capacity = dto.Capacity,
            Floor = dto.Floor,
            RoomName = dto.RoomName,
            IsActive = dto.IsActive,
        };
        await context.Rooms.AddAsync(model);
        await context.SaveChangesAsync();
        var roomDto = new GetRoomDto()
        {
            Id = model.Id,
            Capacity = model.Capacity,
            Floor = model.Floor,
            RoomName = model.RoomName,
            IsActive = model.IsActive,
        };
        return new Response<GetRoomDto>(200, "Room added successfully! ", roomDto);
    }

    public async Task<Response<string>> Update(CreateRoomDto dto)
    {
        var find = await context.Rooms.FindAsync(dto.Id);
        if(find == null) return new Response<string>(404, "Room not found!");
        find.RoomName = dto.RoomName;
        find.Capacity = dto.Capacity;
        find.Floor = dto.Floor;
        find.RoomName = dto.RoomName;
        find.IsActive = dto.IsActive;
        await context.SaveChangesAsync();
        return new Response<string>(200, "Room updated successfully!");
    }

    public async Task<Response<string>> Delete(int id)
    {
        var find = await context.Rooms.FindAsync(id);
        if(find == null) return new Response<string>(404, "Room not found!");
        context.Rooms.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Room deleted successfully!");
    }
}