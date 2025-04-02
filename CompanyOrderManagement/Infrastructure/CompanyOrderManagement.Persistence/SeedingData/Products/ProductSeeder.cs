using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData.Products
{
    public class ProductSeeder : IDataSeeder
    {
        private readonly CompanyOrderManagementDbContext _context;

        public ProductSeeder(CompanyOrderManagementDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context?.Products != null && _context?.ProductCategories != null && _context?.Companies != null && !_context.Products.Any())
            {
                var productCategories = _context.ProductCategories.ToList();
                var companies = _context.Companies.ToList();

                var products = new List<Product>
            {
                new Product {Id = Guid.Parse("7dcbf38d-4e2c-4b4f-9e54-2b7b8e4c8a2b"), Name = "Product1", Description = "Product", CompanyId = companies[0].Id, ProductCategoryId = productCategories[0].Id, Price = 100, Stock = 500, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow},
                new Product {Id = Guid.Parse("7dcbf38d-4e2c-4b4f-9e54-2b7b8e4c8a22"), Name = "Product 2", Description = "Product 2", CompanyId = companies[0].Id, ProductCategoryId = productCategories[1].Id, Price = 1450, Stock = 100, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow}
                };

                _context.Products.AddRange(products);
                _context.SaveChanges();
            }

        }
    }
}
