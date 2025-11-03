namespace Clean.Application.Dtos.Room;

public class CreateRoomDto
{
    public int Id { get; set; }
    public string RoomName { get; set; } = "";
    public int Capacity { get; set; }
    public int Floor { get; set; }
    public int CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
}