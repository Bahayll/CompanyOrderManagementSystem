using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Persistence.Concretes.CompanyRepository
{
    public class CompanyUnitOfWork : UnitOfWork<ICompanyReadRepository, ICompanyWriteRepository, Company> ,ICompanyUnitOfWork
    {
        public CompanyUnitOfWork(CompanyOrderManagementDbContext context, ICompanyReadRepository readRepository, ICompanyWriteRepository writeRepository) : base(context, readRepository, writeRepository)
        {
        }
    }
}
