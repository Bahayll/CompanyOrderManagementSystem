using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.EntityTypeConfigurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.EntityTypeConfigurations.Entities
{
    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override void EntityConfiguration(EntityTypeBuilder<Product> builder)
        {

            builder.HasOne(p => p.ProductCategory)
                   .WithMany(pc => pc.Products)
                   .HasForeignKey(p => p.ProductCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
