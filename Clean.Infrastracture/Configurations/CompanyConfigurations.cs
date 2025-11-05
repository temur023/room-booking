using Clean.Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clean.Infrastracture.Configurations;

public class CompanyConfigurations:IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(60);
    }
}