using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.EntityTypeConfigurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.EntityTypeConfigurations.Entities
{
    public class ProductCategoryConfiguration : BaseConfiguration<ProductCategory>
    {
        public override void EntityConfiguration(EntityTypeBuilder<ProductCategory> builder)
        {
            
        }
    }
}
