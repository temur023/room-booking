using Clean.Application.Abstractions;
using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastracture.Data;

public class DataContext:DbContext,IDbContext
{
    public DbSet<Comapany> Comapanies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomEquipment> RoomEquipments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}