using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Location.Entities;

namespace Services.Location.Configurations;

public class PrefectureConfiguration : IEntityTypeConfiguration<Prefecture>
{
    public void Configure(EntityTypeBuilder<Prefecture> builder)
    {
        builder.HasIndex(p => p.NameJP).IsUnique();  //Set unique index
        builder.HasIndex(p => p.Name).IsUnique();
    }
}