using CompanyOrderManagement.Application.Repositories;
using CompanyOrderManagement.Domain.Entities.Common;
using CompanyOrderManagement.Persistence.Contexts;

public class UnitOfWork<TIReadRepository, TIWriteRepository, TEntity> : IUnitOfWork<TIReadRepository, TIWriteRepository, TEntity>
    where TIReadRepository : IReadRepository<TEntity>
    where TIWriteRepository : IWriteRepository<TEntity>
    where TEntity : BaseEntity    
{
    private readonly CompanyOrderManagementDbContext _context;
    private readonly TIReadRepository _readRepository;
    private readonly TIWriteRepository _writeRepository;

    public UnitOfWork(CompanyOrderManagementDbContext context, TIReadRepository readRepository, TIWriteRepository writeRepository)
    {
        _context = context;
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public TIReadRepository GetReadRepository => _readRepository;

    public TIWriteRepository GetWriteRepository => _writeRepository;

    public int Save() => _context.SaveChanges();

    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

}
