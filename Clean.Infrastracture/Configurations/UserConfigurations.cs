using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastracture.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FullName).IsRequired().HasMaxLength(60);
        builder.HasOne(u => u.Company).WithMany(c=>c.Users).HasForeignKey(u=>u.CompanyId);
        builder.Property(u => u.PasswordHash).IsRequired();
    }
}
