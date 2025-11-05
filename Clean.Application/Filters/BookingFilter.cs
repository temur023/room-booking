namespace Clean.Application.Filters;

public class BookingFilter
{
    public int? UserId{ get; set; }
    public int? RoomId { get; set; }
    public int? Id { get; set; }
    public DateOnly? Date { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}