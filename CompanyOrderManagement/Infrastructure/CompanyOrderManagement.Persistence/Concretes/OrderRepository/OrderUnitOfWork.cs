using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.Concretes.OrderRepository
{
    public class OrderUnitOfWork : UnitOfWork<IOrderReadRepository, IOrderWriteRepository, Order>, IOrderUnitOfWork
    {
        public OrderUnitOfWork(CompanyOrderManagementDbContext context, IOrderReadRepository readRepository, IOrderWriteRepository writeRepository) : base(context, readRepository, writeRepository)
        {
        }
    }
}
