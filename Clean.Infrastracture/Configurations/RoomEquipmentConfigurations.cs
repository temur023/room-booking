using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Clean.Domain.Enitities;
namespace Clean.Infrastracture.Configurations;

public class RoomEquipmentConfigurations:IEntityTypeConfiguration<RoomEquipment>
{
    public void Configure(EntityTypeBuilder<RoomEquipment> builder)
    {
        builder.ToTable("RoomEquipments");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(60);
        builder.HasOne(r => r.Room).WithMany(x => x.Equipments).HasForeignKey(r => r.RoomId);
    }
}