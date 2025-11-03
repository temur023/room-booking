using Clean.Domain.Entities;

namespace Clean.Domain.Enitities;

public class User
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string UserName { get; set; }
    public Company Company { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}