using CompanyOrderManagement.Domain.Exceptions;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using CompanyOrderManagement.Application.Features.Companies.ConstantMessages;


namespace CompanyOrderManagement.Application.Rules.Companies
{
    public class CompanyBusinessRules : ICompanyBusinessRules
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;

        public CompanyBusinessRules(ICompanyUnitOfWork companyUnitOfWork)
        {
            _companyUnitOfWork = companyUnitOfWork;
        }

        public async Task EnsureUniqueCompanyNameAsync(string name)
        {
            var company = await _companyUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());

            if (company != null)
                throw new BusinessException(CompanyMessages.CompanyNameExists);
        }

        public async Task EnsureCompanyIdExists(Guid id)
        {
            var company = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(id);
            if (company == null)
                throw new BusinessException(CompanyMessages.CompanyIdNotExists);
        }

        public async Task CompanyNameCanNotBeDuplicatedWhenUpdated(string name, Guid id)
        {
            var existingCompany = await _companyUnitOfWork.GetReadRepository.GetAll().FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != id);
            if(existingCompany != null)
            throw new BusinessException(CompanyMessages.CompanyNameAlreadyExists);
        }

        public async Task EnsureCompanyDetailsAreUpdated(UpdateCompanyCommandRequest request)
        {
            var detailCompany = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
            if (detailCompany != null && detailCompany.Description == request.Description && detailCompany.Email == request.Email)
                throw new BusinessException(CompanyMessages.CompanyDetailsUpToDate);
        }
    }
}
