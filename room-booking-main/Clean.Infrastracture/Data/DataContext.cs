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

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        
        
        modelBuilder.Entity<User>().HasData(new Domain.Enitities.User
        {
            Id = 1,
            UserName = "superadmin",
            FullName = "Super Admin",
            PasswordHash = "$2a$11$NT4SrHDw6QXy3/7kgVyumupgbY21LY6UuoEnD360ytPKHxl9DX8yu",
            Role = Domain.Entities.Role.Admin, // make sure you have an enum
            CompanyId = 1 // assign to a default company
        });
        
    }
    
}