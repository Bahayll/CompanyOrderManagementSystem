using CompanyOrderManagement.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Repositories
{
    public interface IUnitOfWork <TIReadRepository, TIWriteRepository, TEntity>
        where TIReadRepository : IReadRepository<TEntity>
        where TIWriteRepository : IWriteRepository<TEntity>
        where TEntity : BaseEntity
    {
        TIReadRepository GetReadRepository {  get; }
        TIWriteRepository GetWriteRepository { get; }

        Task<int> SaveAsync();
        int Save();
    }
}
