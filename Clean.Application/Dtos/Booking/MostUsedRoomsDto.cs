namespace Clean.Application.Dtos.Company;

public class MostUsedRoomsDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public int UsedTimes { get; set; }
}