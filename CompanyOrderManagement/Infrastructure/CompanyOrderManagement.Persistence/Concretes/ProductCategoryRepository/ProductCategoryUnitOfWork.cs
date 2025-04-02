using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.Concretes.ProductCategoryRepository
{
    public class ProductCategoryUnitOfWork : UnitOfWork<IProductCategoryReadRepository, IProductCategoryWriteRepository, ProductCategory>, IProductCategoryUnitOfWork
    {
        public ProductCategoryUnitOfWork(CompanyOrderManagementDbContext context, IProductCategoryReadRepository readRepository, IProductCategoryWriteRepository writeRepository) : base(context, readRepository, writeRepository)
        {
        }
    }
}
