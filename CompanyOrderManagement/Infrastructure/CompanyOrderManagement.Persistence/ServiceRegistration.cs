using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CompanyOrderManagement.Persistence.Contexts;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Persistence.Concretes.CompanyRepository;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Persistence.Concretes.OrderRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Persistence.Concretes.ProductRepository;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Persistence.Concretes.ProductCategoryRepository;

using CompanyOrderManagement.Domain.Entities.Identity;

namespace CompanyOrderManagement.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
           services.AddDbContext<CompanyOrderManagementDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<CompanyOrderManagementDbContext>();
            
            services.AddScoped<ICompanyReadRepository,CompanyReadRepository>();
            services.AddScoped<ICompanyWriteRepository, CompanyWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddScoped<IProductCategoryReadRepository, ProductCategoryReadRepository>();
            services.AddScoped<IProductCategoryWriteRepository, ProductCategoryWriteRepository>();


            
            services.AddScoped<ICompanyUnitOfWork, CompanyUnitOfWork>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
            services.AddScoped<IProductCategoryUnitOfWork, ProductCategoryUnitOfWork>();
            services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();

          
          



        }
    }
}
