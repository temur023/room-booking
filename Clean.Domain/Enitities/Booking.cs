namespace Clean.Domain.Enitities;

public class Booking
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public int RoomId { get; set; }
    public DateTime Time { get; set; }
}