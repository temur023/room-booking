using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastracture.Configurations;

public class BookingConfigruations:IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);
        builder.HasOne(b=>b.Room).WithMany(r=>r.Bookings).HasForeignKey(b=>b.RoomId);
        builder.Property(b => b.Date).IsRequired();
    }
}