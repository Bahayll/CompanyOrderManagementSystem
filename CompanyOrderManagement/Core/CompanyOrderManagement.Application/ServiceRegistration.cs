using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Logging.Services;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Logging.Services.Concretes;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Rules.Products;
using CompanyOrderManagement.Application.Rules.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CompanyOrderManagement.Application
{
    public static class ServiceRegistration
    {

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceRegistration));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddScoped<ICompanyBusinessRules, CompanyBusinessRules>();
            services.AddScoped<IProductBusinessRules, ProductBusinessRules>();
            services.AddScoped<IUserBusinessRules, UserBusinessRules>();
            services.AddScoped<IOrderBusinessRules, OrderBusinessRules>();
            services.AddScoped<IProductCategoryBusinessRules, ProductCategoryBusinessRules>();

            services.AddSingleton<ILoggerService, ConsoleLogger>();

            /*
            services.AddSingleton<ConsoleLogger>();
            services.AddSingleton<FileLogger>();
            services.AddSingleton<DatabaseLogger>();

             services.AddSingleton<ILoggerService,CompositeLogger>();
            */
        }
    }
}
