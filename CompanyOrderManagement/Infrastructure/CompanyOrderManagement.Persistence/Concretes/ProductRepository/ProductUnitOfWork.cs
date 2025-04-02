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
    public class ProductUnitOfWork : UnitOfWork<IProductReadRepository, IProductWriteRepository, Product>, IProductUnitOfWork
    {
        public ProductUnitOfWork(CompanyOrderManagementDbContext context, IProductReadRepository readRepository, IProductWriteRepository writeRepository) : base(context, readRepository, writeRepository)
        {
        }
    }
}
