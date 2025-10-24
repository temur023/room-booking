namespace Clean.Domain.Enitities;

public class Comapany
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    ICollection<Room> Rooms { get; set; } = new List<Room>();
    ICollection<User> Users { get; set; } = new List<User>();
}