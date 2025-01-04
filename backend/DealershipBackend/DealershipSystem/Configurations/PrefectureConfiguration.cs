using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DealershipSystem.Configurations;

public class PrefectureConfiguration : IEntityTypeConfiguration<Prefecture>
{
    public void Configure(EntityTypeBuilder<Prefecture> builder)
    {
        builder.HasIndex(p => p.NameJP).IsUnique();  //Set unique index
        builder.HasIndex(p => p.Name).IsUnique();
    }
}