using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData.ProductCategories
{
    public class ProductCategorySeeder : IDataSeeder
    {
        private readonly CompanyOrderManagementDbContext _context;

        public ProductCategorySeeder(CompanyOrderManagementDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context?.ProductCategories != null && !_context.ProductCategories.Any())
            {
                var productCategories = new List<ProductCategory>()
                {
                    new ProductCategory {Id=Guid.Parse("c8c8d1f0-58c2-4a67-9c73-9b6b6e3c6c10"),Name="Web Yazýlým",Description="",CreatedDate=DateTime.UtcNow,LastUpdatedDate=DateTime.UtcNow},
                    new ProductCategory {Id=Guid.Parse("9d1a3e6e-5f68-4bcb-bac5-bcba9c7a7ec2"),Name="RAM",Description="",CreatedDate=DateTime.UtcNow,LastUpdatedDate=DateTime.UtcNow}
                };
                _context.ProductCategories.AddRange(productCategories);
                _context.SaveChanges();
            }
        }
    }
}
