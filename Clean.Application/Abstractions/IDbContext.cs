using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Abstractions;

public interface IDbContext
{
    public DbSet<Company> Comapanies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomEquipment> RoomEquipments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    Task MigrateAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}