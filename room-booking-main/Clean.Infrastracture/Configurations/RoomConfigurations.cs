using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastracture.Configurations;

public class RoomConfigurations:IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.RoomName).HasMaxLength(60);
        builder.HasOne(r => r.Company).WithMany(c => c.Rooms).HasForeignKey(r => r.CompanyId);
    }
}