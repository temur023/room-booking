using Clean.Application.Dtos.Booking;
using Clean.Application.Dtos.Company;
using Clean.Application.Filters;
using Clean.Application.Responses;

namespace Clean.Application.Services.Booking;

public interface IBookingService
{
    Task<PagedResponse<BookingGetDto>> GetAll(BookingFilter filter);
    Task<Response<BookingGetDto>> Add(BookingCreateDto dto);
    Task<Response<string>> Update(BookingCreateDto dto);
    Task<Response<string>> Delete(int id);

    Task<PagedResponse<BookingGetDtoUser>> GetMyBookingsForUser(BookingFilter filter);
    Task<Response<BookingGetDtoUser>> AddForUser(BookingCreateDto dto);
    Task<Response<List<MostUsedRoomsDto>>> GetMostUsedRooms();
    Task<Response<List<UserActivityDto>>> GetUserActivity();
    Task<Response<List<BookingTrendDto>>> GetBookingTrends();

    Task<Response<string>> UpdateForUser(BookingCreateDto dto);
    Task<Response<string>> DeleteForUser(int id);
}