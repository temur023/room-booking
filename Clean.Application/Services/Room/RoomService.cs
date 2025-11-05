using System.Security.Claims;
using Clean.Application.Abstractions;
using Clean.Application.Dtos.Company;
using Clean.Application.Dtos.Room;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Room;

public class RoomService(IDbContext context, IHttpContextAccessor httpContextAccessor) : IRoomService
{
    private Role? GetCurrentRole() =>
        Enum.TryParse<Role>(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value, out var role) ? role : null;

    private int? GetCompanyId() =>
        int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst("companyId")?.Value, out var companyId) ? companyId : null;

    private bool IsSuperAdmin => GetCurrentRole() == Role.SuperAdmin;

    public async Task<PagedResponse<GetRoomDto>> GetAll(RoomFilter filter)
    {
        var query = context.Rooms.Include(r => r.Equipments).AsQueryable();

        if (!IsSuperAdmin)
        {
            var companyId = GetCompanyId();
            query = query.Where(r => r.CompanyId == companyId);
        }

        if (filter.MinCapacity.HasValue)
            query = query.Where(r => r.Capacity >= filter.MinCapacity);

        if (filter.MaxCapacity.HasValue)
            query = query.Where(r => r.Capacity <= filter.MaxCapacity);

        if (filter.Floor.HasValue)
            query = query.Where(r => r.Floor == filter.Floor);
        

        if (filter.Equipments?.Any() == true)
        {
            query = query.Where(r => r.Equipments
                .Any(e => filter.Equipments
                    .Any(eq => e.Name.ToLower() == eq.ToLower())));
        }

        var totalCount = await query.CountAsync();
        var rooms = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        var roomDto = rooms.Select(r => new GetRoomDto
        {
            Id = r.Id,
            Capacity = r.Capacity,
            Floor = r.Floor,
            RoomName = r.RoomName,
            Equipments = r.Equipments
        }).ToList();

        return new PagedResponse<GetRoomDto>(
            data: roomDto,
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize,
            totalRecords: totalCount,
            message: "Rooms retrieved successfully!"
        );
    }
    public async Task<Response<List<GetAvailableDto>>> GetAvailableRooms(DateOnly date, int startBlock, int endBlock)
    {
        // Base query (with company filter if not super admin)
        var query = context.Rooms
            .Include(r => r.Equipments)
            .AsQueryable();

        if (!IsSuperAdmin)
        {
            var companyId = GetCompanyId();
            query = query.Where(r => r.CompanyId == companyId);
        }

        // Get available rooms (no overlapping bookings)
        var availableRooms = await query
            .Where(r => !context.Bookings.Any(b =>
                b.RoomId == r.Id &&
                b.Date == date &&
                b.StartBlock < endBlock &&
                b.EndBlock > startBlock))
            .Select(r => new GetAvailableDto
            {
                Id = r.Id,
                RoomName = r.RoomName,
                Capacity = r.Capacity,
                Floor = r.Floor
            })
            .ToListAsync();

        return new Response<List<GetAvailableDto>>(200, "Available Rooms:", availableRooms);
    }

    public async Task<PagedResponse<GetRoomDto>> GetByName(string roomName, RoomFilter filter)
    {
        var query = context.Rooms.Include(r => r.Equipments).AsQueryable();
        if (!IsSuperAdmin)
        {
            var company = GetCompanyId();
            query = query.Where(r => r.CompanyId == company);
        }   
        if (!string.IsNullOrWhiteSpace(roomName))
        {
            roomName = roomName.ToLower();
            query = query.Where(r => r.RoomName.ToLower().Contains(roomName));
        }
        var totalCount = await query.CountAsync();
        var rooms = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        var dto = rooms.Select(r => new GetRoomDto
        {
            Id = r.Id,
            RoomName = r.RoomName,
            Capacity = r.Capacity,
            Floor = r.Floor,
            Equipments = r.Equipments
        }).ToList();
        return new PagedResponse<GetRoomDto>(dto, filter.PageNumber, filter.PageSize, totalCount);
    }
    
    public async Task<Response<GetRoomDto>> GetById(int id)
    {
        var room = await context.Rooms.Include(r => r.Equipments).FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) return new Response<GetRoomDto>(404, "Room not found");

        if (!IsSuperAdmin && room.CompanyId != GetCompanyId())
            return new Response<GetRoomDto>(404, "Room not found");

        var dto = new GetRoomDto
        {
            Id = room.Id,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Floor = room.Floor,
            Equipments = room.Equipments
        };

        return new Response<GetRoomDto>(200, "Room retrieved successfully", dto);
    }
    
    public async Task<Response<GetRoomDto>> Add(CreateRoomDto dto)
    {
        if (!IsSuperAdmin)
            dto.CompanyId = GetCompanyId().Value; // Admin can only add rooms for their company

        var room = new Domain.Enitities.Room
        {
            RoomName = dto.RoomName,
            Capacity = dto.Capacity,
            Floor = dto.Floor,
            CompanyId = dto.CompanyId
        };

        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();

        var dtoResult = new GetRoomDto
        {
            Id = room.Id,
            RoomName = room.RoomName,
            Capacity = room.Capacity,
            Floor = room.Floor,
            Equipments = room.Equipments
        };

        return new Response<GetRoomDto>(200, "Room added successfully!", dtoResult);
    }
    
    public async Task<Response<string>> Update(CreateRoomDto dto)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == dto.Id);
        if (room == null) return new Response<string>(404, "Room not found");

        if (!IsSuperAdmin && room.CompanyId != GetCompanyId())
            return new Response<string>(403, "No room found!");

        room.RoomName = dto.RoomName;
        room.Capacity = dto.Capacity;
        room.Floor = dto.Floor;

        if (IsSuperAdmin)
            room.CompanyId = dto.CompanyId; // Only SuperAdmin can change company

        await context.SaveChangesAsync();
        return new Response<string>(200, "Room updated successfully!");
    }
    
    public async Task<Response<string>> Delete(int id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) return new Response<string>(404, "Room not found");

        if (!IsSuperAdmin && room.CompanyId != GetCompanyId())
            return new Response<string>(404, "No room found!");

        context.Rooms.Remove(room);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Room deleted successfully!");
    }
    
}
