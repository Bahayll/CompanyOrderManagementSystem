using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.EntityTypeConfigurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyOrderManagement.Persistence.EntityTypeConfigurations.Entities
{
    public class OrderConfiguration : BaseConfiguration<Order>
    {
        public override void EntityConfiguration(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.Products)
                  .WithMany(p => p.Orders)
                  .UsingEntity<Dictionary<string, object>>(
                       "OrderProduct",
                       j => j
                           .HasOne<Product>()
                           .WithMany()
                           .HasForeignKey("ProductId")
                           .OnDelete(DeleteBehavior.Restrict),
                       j => j
                           .HasOne<Order>()
                           .WithMany()
                           .HasForeignKey("OrderId")
                           .OnDelete(DeleteBehavior.Cascade)
                  );

            builder.Property(o => o.TotalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
        }
    }
}
