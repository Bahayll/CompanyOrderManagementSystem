using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Entities.Common;
using CompanyOrderManagement.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace CompanyOrderManagement.Persistence.Contexts
{
    public class CompanyOrderManagementDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CompanyOrderManagementDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Company>? Companies { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<ProductCategory>? ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);  
         
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        data.Entity.LastUpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        break;
                    default:
                        break;
                }   
            }

            return await base.SaveChangesAsync(cancellationToken);

        }
    }
}
