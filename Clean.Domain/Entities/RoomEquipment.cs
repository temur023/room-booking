namespace Clean.Domain.Enitities;

public class RoomEquipment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
}