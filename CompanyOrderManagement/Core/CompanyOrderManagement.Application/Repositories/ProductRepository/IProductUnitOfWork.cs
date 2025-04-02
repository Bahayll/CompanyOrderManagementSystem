using CompanyOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Repositories.ProductRepository
{
    public interface IProductUnitOfWork : IUnitOfWork<IProductReadRepository,IProductWriteRepository, Product>
    {
    }
}
