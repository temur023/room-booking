namespace Clean.Domain.Enitities;

public class User
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}