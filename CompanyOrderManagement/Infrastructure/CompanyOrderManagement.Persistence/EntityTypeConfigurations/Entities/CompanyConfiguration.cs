using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.EntityTypeConfigurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyOrderManagement.Persistence.EntityTypeConfigurations.Entities
{
    public class CompanyConfiguration : BaseConfiguration<Company>
    {
        public override void EntityConfiguration(EntityTypeBuilder<Company> builder)
        {
            builder.HasMany(c => c.Products)
                  .WithOne(p => p.Company)
                  .HasForeignKey(p => p.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Orders)
                   .WithOne(o => o.Company)
                   .HasForeignKey(o => o.CompanyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
