using CompanyOrderManagement.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CompanyOrderManagement.Persistence.EntityTypeConfigurations.Common
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            EntityConfiguration(builder);
        }

        public abstract void EntityConfiguration(EntityTypeBuilder<TEntity> builder);
        

          
    }
}
