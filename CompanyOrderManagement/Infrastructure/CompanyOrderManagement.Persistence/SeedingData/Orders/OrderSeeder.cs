using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Enums;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData.Orders
{
    public class OrderSeeder : IDataSeeder
    {
        private readonly CompanyOrderManagementDbContext _context;

        public OrderSeeder(CompanyOrderManagementDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context?.Orders != null && _context?.Products != null && _context?.Users != null && _context?.Companies != null && !_context.Orders.Any())
            {
                var products = _context.Products.ToList();
                var users = _context.Users.ToList();
                var companies = _context.Companies.ToList();

                var orders = new List<Order>
            {
                new Order {Id = Guid.Parse("ba99d1a2-34c2-4d47-a8e3-7c62148d8274"), Name = "Order 1", Address = "Istanbul/Turkey", CompanyId = companies[0].Id, Description = "", OrderStatus = OrderStatus.Completed, ProductCount = 10, UnitPrice = 100, TotalPrice = 1000, UserId = users[0].Id, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow, Products = new List<Product> { products[0], products[1] }},
                new Order {Id = Guid.Parse("ba99d1a2-34c2-4d47-a8e3-7c62148d8264"), Name = "Order 2", Address = "Istanbul/Turkey", CompanyId = companies[0].Id, Description = "", OrderStatus = OrderStatus.Processing, ProductCount = 100, UnitPrice = 5, TotalPrice = 500, UserId = users[1].Id, CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow}
            };

                _context.Orders.AddRange(orders);
                _context.SaveChanges();
            }
        }
    }
}