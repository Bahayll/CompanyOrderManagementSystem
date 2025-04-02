using CompanyOrderManagement.Application.Abstractions.Tokens;
using CompanyOrderManagement.Application.Services;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Infrastructure.Services;
using CompanyOrderManagement.Infrastructure.Services.Cache;
using CompanyOrderManagement.Infrastructure.Services.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMailService, MailService>();

            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
        }
    }
}
