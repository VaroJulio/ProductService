using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.ProductAggregate;

namespace ProductService.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Status).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.Stock).IsRequired().HasDefaultValue(default);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Price).IsRequired().HasPrecision(18,2).HasDefaultValue(default);
            builder.HasQueryFilter(p => p.Status.Equals(true));
        }
    }
}
