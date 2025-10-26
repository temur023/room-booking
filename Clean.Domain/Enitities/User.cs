namespace Clean.Domain.Enitities;

public class User
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "Member";
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}