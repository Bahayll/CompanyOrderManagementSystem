using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.Concretes.ProductRepository
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(CompanyOrderManagementDbContext context) : base(context)
        {
        }
    }
}
