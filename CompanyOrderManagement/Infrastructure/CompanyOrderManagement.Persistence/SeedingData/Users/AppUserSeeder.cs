using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Entities.Identity;
using CompanyOrderManagement.Domain.Enums;
using CompanyOrderManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.SeedingData.Users
{
    public class AppUserSeeder : IDataSeeder
    {
        private readonly CompanyOrderManagementDbContext _context;
        private readonly UserManager<AppUser> _manager;

        public AppUserSeeder(CompanyOrderManagementDbContext context, UserManager<AppUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public void Seed()
        {
            if (_context?.Users != null && !_context.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id = Guid.Parse("a6cec149-f87b-43e0-b4e8-43fa24e05c58"),FullName = "Baha YOLAL",UserName="Bahayll", Email = "user1@gmail.com", PhoneNumber = "+905375927782", CreatedDate = DateTime.UtcNow,LastUpdatedDate = DateTime.UtcNow
                    },
                    new AppUser
                    {
                        Id = Guid.Parse("a6cec149-f87b-43e0-b4e8-43fa24e05c52"),FullName = "Ayþe Göncü",UserName="Aysegnc", Email = "user2@gmail.com", PhoneNumber = "+905301101169",CreatedDate = DateTime.UtcNow,LastUpdatedDate = DateTime.UtcNow
                    }
                };
                _manager.CreateAsync(users[0], "SecurePassword123");
                _manager.CreateAsync(users[1], "SecurePassword1234");

                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
