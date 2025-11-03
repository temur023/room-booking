using System.Security.Claims;
using Clean.Application.Abstractions;
using Clean.Application.Dtos.Booking;
using Clean.Application.Dtos.User;
using Clean.Application.Filters;
using Clean.Application.Responses;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Services.Booking;

public class BookingService(IDbContext context, IHttpContextAccessor httpContextAccessor):IBookingService
{
    private Role? GetCurrentRole() =>
        Enum.TryParse<Role>(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value, out var role) ? role : null;

    private int? GetCompanyId() =>
        int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst("companyId")?.Value, out var companyId) ? companyId : null;

    private int? GetCurrentUserId() =>
        int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : null;

    private bool IsSuperAdmin => GetCurrentRole() == Role.SuperAdmin;
    private bool IsAdmin => GetCurrentRole() == Role.Admin;
    public async Task<PagedResponse<BookingGetDto>> GetAll(BookingFilter filter)
    {
        
        var query = context.Bookings.Include(b=>b.Room).Include(b=>b.User).AsQueryable();
        if (!IsSuperAdmin)
        {
            var companyId = GetCompanyId();
            query = query.Where(b => b.User.CompanyId == companyId);
        }
        
        if(filter.UserId.HasValue) query = query.Where(x => x.UserID == filter.UserId);
        if(filter.RoomId.HasValue) query = query.Where(x => x.RoomId == filter.RoomId);
        if(filter.Id.HasValue) query = query.Where(x => x.Id == filter.Id);
        var totalCount = await query.CountAsync();
        var bookings = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var bookingDtos = bookings.Select(b => new BookingGetDto()
        {
            Id = b.Id,
            RoomId = b.RoomId,
            Date = b.Date,
            StartBlock = b.StartBlock,
            EndBlock = b.EndBlock,
            UserID = b.UserID,
        }).ToList();
        return new PagedResponse<BookingGetDto>(
             data: bookingDtos, 
            totalCount,
            filter.PageNumber,
            filter.PageSize,
             message: "Bookings retrieved successfully "
            );
    }

    public async Task<Response<BookingGetDto>> Add(BookingCreateDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var companyId = GetCompanyId();
        
        if (!IsSuperAdmin)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == dto.UserID);
            if (user == null || user.CompanyId != companyId)
                return new Response<BookingGetDto>(403, "Error");
        }

        var model = new Domain.Enitities.Booking()
        {
            RoomId = dto.RoomId,
            Date = dto.Date,
            StartBlock = dto.StartBlock,
            EndBlock = dto.EndBlock,
            UserID = dto.UserID,
        };
        var existingBookings = await context.Bookings
            .Where(b => b.RoomId == model.RoomId && b.Date == model.Date)
            .ToListAsync();
        
        if (HasConflict(model, existingBookings))
        {
            return new Response<BookingGetDto>(409, " This time slot is already booked.");
        }
        context.Bookings.Add(model);
        await context.SaveChangesAsync();
        var booking = new BookingGetDto
        {
            Id = model.Id,
            RoomId = model.RoomId,
            Date = model.Date,
            StartBlock = model.StartBlock,
            EndBlock = model.EndBlock,
            UserID = model.UserID,
        };
        return new Response<BookingGetDto>(200,"Booking added successfully", booking);
    }

    public async Task<Response<string>> Update(BookingCreateDto dto)
    {
        var find = await context.Bookings.Include(b=>b.User).FirstOrDefaultAsync(b => b.Id == dto.Id);
        if(find == null) return new Response<string>(404,"Booking not found");
        
        if (!IsSuperAdmin && find.User.CompanyId != GetCompanyId())
            return new Response<string>(403, "Error");
        
        find.RoomId = dto.RoomId;
        find.Date = dto.Date;
        find.StartBlock = dto.StartBlock;
        find.EndBlock = dto.EndBlock;
        
        var existingBookings = await context.Bookings
            .Where(b => b.RoomId == dto.RoomId && b.Date == dto.Date && b.Id != dto.Id)
            .ToListAsync();
        
        if (HasConflict(find, existingBookings))
        {
            return new Response<string>(409, " This time slot is already booked.");
        }
        
        await context.SaveChangesAsync();
        return new Response<string>(200, "Booking updated successfully");
    }

    public async Task<Response<string>> Delete(int id)
    {
        var find = await context.Bookings.Include(b=>b.User).FirstOrDefaultAsync(b => b.Id == id);
        
        
        if(find == null) return new Response<string>(404,"Booking not found");
        
        if (!IsSuperAdmin && find.User.CompanyId != GetCompanyId())
            return new Response<string>(403, "Forbidden: Cannot delete booking of another company");
        context.Bookings.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Booking deleted successfully");
    }

    //For User
    public async Task<PagedResponse<BookingGetDto>> GetMyBookings(BookingFilter filter)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return new PagedResponse<BookingGetDto>(
                data: new List<BookingGetDto>(),
                totalRecords: 0,
                pageNumber: filter.PageNumber,
                pageSize: filter.PageSize,
                message: "Unauthorized: user not found in token."
            );
        var query = context.Bookings.Include(b=>b.Room).Include(b=>b.User).Where(b=>b.UserID == currentUserId).AsQueryable();
        if(filter.RoomId.HasValue) query = query.Where(x => x.RoomId == filter.RoomId);
        if(filter.Id.HasValue) query = query.Where(x => x.Id == filter.Id);
        if(filter.Date.HasValue) query = query.Where(x=>x.Date == filter.Date);
            
        var totalCount = await query.CountAsync();
        var bookings = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var bookingDtos = bookings.Select(b => new BookingGetDto()
        {
            Id = b.Id,
            RoomId = b.RoomId,
            Date = b.Date,
            StartBlock = b.StartBlock,
            EndBlock = b.EndBlock,
            UserID = b.UserID,
        }).ToList();
        return new PagedResponse<BookingGetDto>(
            data: bookingDtos, 
            totalCount,
            filter.PageNumber,
            filter.PageSize,
            message: "Bookings retrieved successfully "
        );
    }
    
    public async Task<Response<BookingGetDto>> AddForUser(BookingCreateDto dto)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return new Response<BookingGetDto>(401, "Unauthorized: user not found in token.");
        var companyId = GetCompanyId();
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == dto.RoomId);
        if (room == null)
            return new Response<BookingGetDto>(404, "Room not found.");

        if (room.CompanyId != companyId)
            return new Response<BookingGetDto>(403, "Error");

        var model = new Domain.Enitities.Booking()
        {
            RoomId = dto.RoomId,
            Date = dto.Date,
            StartBlock = dto.StartBlock,
            EndBlock = dto.EndBlock,
            UserID = currentUserId.Value,
        };
        var existingBookings = await context.Bookings
            .Where(b => b.RoomId == model.RoomId && b.Date == model.Date)
            .ToListAsync();
        
        if (HasConflict(model, existingBookings))
        {
            return new Response<BookingGetDto>(409, " This time slot is already booked.");
        }
        context.Bookings.Add(model);
        await context.SaveChangesAsync();
        var booking = new BookingGetDto
        {
            Id = model.Id,
            RoomId = model.RoomId,
            Date = model.Date,
            StartBlock = model.StartBlock,
            EndBlock = model.EndBlock,
            UserID = model.UserID,
        };
        return new Response<BookingGetDto>(200,"Booking added successfully", booking);
    }
    public async Task<Response<string>> UpdateForUser(BookingCreateDto dto)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return new Response<string>(401, "Unauthorized: user not found in token.");
        var companyId = GetCompanyId();
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == dto.RoomId);
        if (room == null)
            return new Response<string>(404, "Room not found.");

        if (room.CompanyId != companyId)
            return new Response<string>(403, "Error");
        var find = await context.Bookings.Include(b=>b.User).FirstOrDefaultAsync(b => b.Id == dto.Id);
        if(find == null) return new Response<string>(404,"Booking not found");
        
        if (find.UserID != currentUserId)
            return new Response<string>(403, "Error");

        find.RoomId = dto.RoomId;
        find.Date = dto.Date;
        find.StartBlock = dto.StartBlock;
        find.EndBlock = dto.EndBlock;
        
        var existingBookings = await context.Bookings
            .Where(b => b.RoomId == dto.RoomId && b.Date == dto.Date && b.Id != dto.Id)
            .ToListAsync();
        
        if (HasConflict(find, existingBookings))
        {
            return new Response<string>(409, " This time slot is already booked.");
        }
        
        await context.SaveChangesAsync();
        return new Response<string>(200, "Booking updated successfully");
    }
    public async Task<Response<string>> DeleteForUser(int id)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return new Response<string>(401, "Unauthorized: user not found in token.");
        var companyId = GetCompanyId();
        
        
        var find = await context.Bookings.Include(b=>b.User).FirstOrDefaultAsync(b => b.Id == id);
        if(find == null) return new Response<string>(404,"Booking not found");
        
        if (find.User.CompanyId != companyId)
            return new Response<string>(403, "Error");
        
        context.Bookings.Remove(find);
        await context.SaveChangesAsync();
        return new Response<string>(200, "Booking deleted successfully");
    }
    private bool HasConflict(Domain.Enitities.Booking newBooking, List<Domain.Enitities.Booking> existingBookings)
    {
        return existingBookings.Any(b =>
            b.RoomId == newBooking.RoomId &&
            b.Date == newBooking.Date &&
            !(newBooking.EndBlock <= b.StartBlock || newBooking.StartBlock >= b.EndBlock));
    }
}