using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Entities.Identity;
using CompanyOrderManagement.Persistence.Contexts;
using CompanyOrderManagement.Persistence.SeedingData.Companies;
using CompanyOrderManagement.Persistence.SeedingData.Orders;
using CompanyOrderManagement.Persistence.SeedingData.ProductCategories;
using CompanyOrderManagement.Persistence.SeedingData.Products;
using CompanyOrderManagement.Persistence.SeedingData.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData
{
    public class SeedData
    {
        private readonly CompanyOrderManagementDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public SeedData(CompanyOrderManagementDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seeding()
        {
            var seeders = new IDataSeeder[]
            {
                new CompanySeeder(_context),
                new ProductCategorySeeder(_context),
                new AppUserSeeder(_context,_userManager),
                new ProductSeeder(_context),
                new OrderSeeder(_context),              
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed();
            }
        }
    }

}