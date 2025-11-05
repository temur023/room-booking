namespace Clean.Domain.Enitities;

public class UserPreference
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? PreferedCapacity  { get; set; }
    public int? PreferedFloor { get; set; } 
    
    public ICollection<RoomEquipment> PreferedEquipments { get; set; } = new List<RoomEquipment>();
}