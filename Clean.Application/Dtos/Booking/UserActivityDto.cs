namespace Clean.Application.Dtos.Booking;

public class UserActivityDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public int TotalHours { get; set; }
}