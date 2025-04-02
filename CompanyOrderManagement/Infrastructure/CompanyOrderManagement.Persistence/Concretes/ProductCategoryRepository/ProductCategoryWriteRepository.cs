using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.Concretes.ProductCategoryRepository
{
    public class ProductCategoryWriteRepository : WriteRepository<ProductCategory>, IProductCategoryWriteRepository
    {
        public ProductCategoryWriteRepository(CompanyOrderManagementDbContext context) : base(context)
        {
        }
    }
}
