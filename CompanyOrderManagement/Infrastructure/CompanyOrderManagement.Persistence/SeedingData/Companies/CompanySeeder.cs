using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData.Companies
{
    public class CompanySeeder : IDataSeeder
    {
        private readonly CompanyOrderManagementDbContext _context;

        public CompanySeeder(CompanyOrderManagementDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context?.Companies != null && !_context.Companies.Any())
            {
                var companies = new List<Company>
            {
                new Company {Id = Guid.Parse("aeb63e27-4fc8-4c51-88da-6b4a2b99b6e0"), Name = "Enoca 1", Description = "Yazýlým", Email = "enoca1@enoca.com", CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow},
                new Company {Id = Guid.Parse("aeb63e27-4fc8-4c51-88da-6b4a2b99b6e2"), Name = "Enoca 2", Description = "Donaným", Email = "enoca2@enoca.com", CreatedDate = DateTime.UtcNow, LastUpdatedDate = DateTime.UtcNow}
            };
                _context.Companies.AddRange(companies);
                _context.SaveChanges();
            }
        }
    }
}
