namespace Clean.Domain.Enitities;

public class Booking
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public User User { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public DateOnly Date { get; set; }
    
    public int StartBlock { get; set; }
    
    public int EndBlock { get; set; }
}