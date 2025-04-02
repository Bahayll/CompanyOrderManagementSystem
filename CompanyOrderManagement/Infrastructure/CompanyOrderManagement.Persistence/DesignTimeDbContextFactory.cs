using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CompanyOrderManagement.Persistence.Contexts;
using Microsoft.IdentityModel.Protocols;
using CompanyOrderManagement.Persistence;

namespace Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CompanyOrderManagementDbContext>
{
    public CompanyOrderManagementDbContext CreateDbContext(string[] args)

    {
        DbContextOptionsBuilder<CompanyOrderManagementDbContext> dbContextOptionsBuilder = new();
        dbContextOptionsBuilder.UseSqlServer(Configuration.ConnectionString);
        return new(dbContextOptionsBuilder.Options);
    }
}