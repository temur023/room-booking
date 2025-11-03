using Clean.Application.Dtos.Booking;
using Clean.Application.Filters;
using Clean.Application.Responses;

namespace Clean.Application.Services.Booking;

public interface IBookingService
{
    Task<PagedResponse<BookingGetDto>> GetAll(BookingFilter filter);
    Task<Response<BookingGetDto>> Add(BookingCreateDto dto);
    Task<Response<string>> Update(BookingCreateDto dto);
    Task<Response<string>> Delete(int id);
}