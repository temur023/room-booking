namespace Clean.Domain.Enitities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<User> Users { get; set; } = new List<User>();
}