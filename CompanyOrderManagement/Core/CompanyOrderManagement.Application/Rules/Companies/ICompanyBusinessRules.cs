using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using CompanyOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.Rules.Companies
{
    public interface ICompanyBusinessRules
    {
        Task EnsureCompanyIdExists(Guid id);
        Task EnsureUniqueCompanyNameAsync(string name);
        Task CompanyNameCanNotBeDuplicatedWhenUpdated(string name, Guid id);
        Task EnsureCompanyDetailsAreUpdated(UpdateCompanyCommandRequest request);


    }
}
