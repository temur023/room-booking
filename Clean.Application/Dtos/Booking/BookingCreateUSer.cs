namespace Clean.Application.Dtos.Booking;

public class BookingCreateUSer
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public DateOnly Date { get; set; }
    public int StartBlock { get; set; }

    public int EndBlock { get; set; }
}