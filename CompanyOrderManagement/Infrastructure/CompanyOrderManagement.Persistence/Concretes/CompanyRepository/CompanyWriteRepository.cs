using CompanyOrderManagement.Application.Repositories;
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
    public class CompanyWriteRepository : WriteRepository<Company>, ICompanyWriteRepository
    {
        public CompanyWriteRepository(CompanyOrderManagementDbContext context) : base(context)
        {
        }
    }
}
