using Clean.Application.Abstractions;
using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Clean.Infrastracture.Data;

public class DataContext:DbContext,IDbContext
{
    public DbSet<Company> Comapanies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomEquipment> RoomEquipments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "host=localhost; port=5432;Database=RoomBookingDb; Username=postgres; password=2323",
                b => b.MigrationsAssembly("Clean.Infrastracture")
            );
        }
    }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        
        
    }
    
}