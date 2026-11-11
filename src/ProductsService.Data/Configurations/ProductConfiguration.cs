using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsService.Data.Common.Models;
using ProductsService.Data.Entities;

namespace ProductsService.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .UseIdentityByDefaultColumn()
            .HasIdentityOptions(startValue: 100_000, incrementBy: 1, maxValue: 999_999, cyclic: false);
        
        builder.HasIndex(e => e.Name)
            .IsUnique();
    
        builder.Property(e => e.Name)
            .HasMaxLength(MaxLength.C64)
            .IsRequired();
    
        builder.Property(e => e.Description)
            .HasMaxLength(MaxLength.C256);
        
        builder.Property(e => e.Price)
            .HasPrecision(18, 2)
            .IsRequired();
        
        builder.Property(e => e.Stock)
            .IsConcurrencyToken()
            .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);
        
        builder.ToTable(table =>
        {
            table.HasCheckConstraint("ck_products_id_six_digits", "id BETWEEN 100000 AND 999999");
            table.HasCheckConstraint("ck_products_price_greater_than_zero", "price > 0");
            table.HasCheckConstraint("ck_products_stock_non_negative", "stock >= 0");
        });
        
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.SeedFromJsonFile();
    }
}