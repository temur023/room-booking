namespace Clean.Application.Dtos.Room;

public class GetAvailableDto
{
    public int Id { get; set; }
    public string RoomName { get; set; } = "";
    public int Capacity { get; set; }
    public int Floor { get; set; }
    
    public ICollection<Domain.Enitities.RoomEquipment>? Equipments { get; set; } = new List<Domain.Enitities.RoomEquipment>();
}