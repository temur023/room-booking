namespace Clean.Domain.Enitities;

public class Room
{
    public int Id { get; set; }
    public string RoomName { get; set; } = "";
    public int Capacity { get; set; }
    public int Floor { get; set; }
    public bool IsActive { get; set; } = true;

    public int CompanyId { get; set; }
    
    public Company Company { get; set; }
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<RoomEquipment> Equipments { get; set; } = new List<RoomEquipment>();
}