using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Data.Common.Models;
using ProductsService.Data.Entities;

namespace ProductsService.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .UseIdentityByDefaultColumn();

        builder.Property(c => c.Name)
            .HasMaxLength(MaxLength.C64)
            .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);
        
        builder.SeedFromJsonFile();
    }
}