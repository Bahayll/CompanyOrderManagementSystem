using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using CompanyOrderManagement.Application.Features.Companies.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Companies.Commands
{
    
    public class UpdateCompanyCommandHandlerTest
    {
        private readonly Mock<ICompanyUnitOfWork> _companyUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICompanyBusinessRules> _businessRulesMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly UpdateCompanyCommandHandler _updateCompanyCommandHandler;
        private readonly Mock<ICompanyReadRepository> _companyReadRepositoryMock;

        public UpdateCompanyCommandHandlerTest()
        {
            _companyReadRepositoryMock = new Mock<ICompanyReadRepository>();
            _companyUnitOfWorkMock = new Mock<ICompanyUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _businessRulesMock = new Mock<ICompanyBusinessRules>();

            _companyUnitOfWorkMock.Setup(x => x.GetReadRepository)
               .Returns(_companyReadRepositoryMock.Object);

            _updateCompanyCommandHandler = new UpdateCompanyCommandHandler(_companyUnitOfWorkMock.Object, _businessRulesMock.Object, _mapperMock.Object, _loggerServiceMock.Object, _cacheServiceMock.Object);
        }
        [Fact]
        public async Task UpdateCompanyCommandHandler_Handle_ShouldReturnUpdatedCompany()
        {
            // Arrange
            var request = new UpdateCompanyCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Company",
                Description = "Updated Description",
                Email = "updated@gmail.com"
            };

            var company = new Company
            {
                Id = request.Id,
                Name = "Company",
                Description = "Description",
                Email = "company@gmail.com"
            };

            var response = new UpdateCompanyCommandResponse
            {
                Id= request.Id,
                Name = request.Name,
                Description = request.Description,
                Email = request.Email,
                CreatedDate = company.CreatedDate,
                LastUpdatedDate = DateTime.UtcNow
            };

            _businessRulesMock.Setup(cu => cu.EnsureCompanyIdExists(request.Id)).Returns(Task.CompletedTask);
            _businessRulesMock.Setup(cu => cu.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name,request.Id)).Returns(Task.CompletedTask);
            _businessRulesMock.Setup(cu => cu.EnsureCompanyDetailsAreUpdated(request)).Returns(Task.CompletedTask);

            _companyReadRepositoryMock.Setup(cu => cu.GetByIdAsync(request.Id, It.IsAny<bool>())).ReturnsAsync(company);
            _mapperMock.Setup(cu => cu.Map(request, company)).Returns(It.IsAny<Company>);
            _companyUnitOfWorkMock.Setup(cu => cu.GetWriteRepository.Update(company)).Returns(true);
            _companyUnitOfWorkMock.Setup(cu => cu.SaveAsync()).ReturnsAsync(1);
            _mapperMock.Setup(cu => cu.Map<UpdateCompanyCommandResponse>(company)).Returns(response);


            // Act
            var result = await _updateCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.Equal(response,result.Data);
            Assert.Equal(response.Name, result.Data.Name);
            Assert.Equal(response.Description, result.Data.Description);
            Assert.Equal(response.Email, result.Data.Email);
            

            _businessRulesMock.Verify(cu => cu.EnsureCompanyIdExists(request.Id), Times.Once);
            _businessRulesMock.Verify(cu => cu.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id), Times.Once);
            _businessRulesMock.Verify(cu => cu.EnsureCompanyDetailsAreUpdated(request), Times.Once);

            _companyReadRepositoryMock.Verify(cu => cu.GetByIdAsync(request.Id,It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(cu => cu.Map(request, company),Times.Once);
            _companyUnitOfWorkMock.Verify(cu => cu.GetWriteRepository.Update(company), Times.Once);
            _companyUnitOfWorkMock.Verify(cu => cu.SaveAsync(), Times.Once);
            _mapperMock.Verify(cu => cu.Map<UpdateCompanyCommandResponse>(company),Times.Once);

            _loggerServiceMock.Verify(logger => logger.Info(It.Is<string>(s => s.Contains($"Company with ID {request.Id} has been successfully updated."))), Times.Once);
  
        }
        [Fact]
        public async Task UpdateCompanyCommandHandler_Handle_WhenCompanyIsUpdated_ShouldUpdateCache()
        {
            // Arrange
            var cacheKey = CacheKeys.AllCompanies;
            var request = new UpdateCompanyCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Company",
                Description = "Updated Description",
                Email = "updated@gmail.com"
            };

            var company = new Company
            {
                Id = request.Id,
                Name = "Company",
                Description = "Description",
                Email = "company@gmail.com"
            };

            var updatedCompany = new Company
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Email = request.Email
            };
            var response = new UpdateCompanyCommandResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Email = request.Email,
                CreatedDate = company.CreatedDate,
                LastUpdatedDate = DateTime.UtcNow
            };
            var cachedCompanies = new List<Company> { company };

            _businessRulesMock.Setup(cu => cu.EnsureCompanyIdExists(request.Id)).Returns(Task.CompletedTask);
            _businessRulesMock.Setup(cu => cu.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).Returns(Task.CompletedTask);
            _businessRulesMock.Setup(cu => cu.EnsureCompanyDetailsAreUpdated(request)).Returns(Task.CompletedTask);
            _companyReadRepositoryMock.Setup(cu => cu.GetByIdAsync(request.Id, It.IsAny<bool>())).ReturnsAsync(company);
            _mapperMock.Setup(cu => cu.Map(request, company)).Callback(() => { company.Name = request.Name; company.Description = request.Description; company.Email = request.Email; });
            _companyUnitOfWorkMock.Setup(cu => cu.GetWriteRepository.Update(company));
            _companyUnitOfWorkMock.Setup(cu => cu.SaveAsync()).ReturnsAsync(1);
            _mapperMock.Setup(cu => cu.Map<UpdateCompanyCommandResponse>(company)).Returns(response);

            _cacheServiceMock.Setup(c => c.Get<List<Company>>(cacheKey)).Returns(cachedCompanies);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null));


            // Act
            await _updateCompanyCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            _cacheServiceMock.Verify(cc => cc.Get<List<Company>>(It.Is<string>(key => key == cacheKey)), Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<Company>>(), It.IsAny<TimeSpan>(), null), Times.Once);

        }
      

        [Fact]
        public async Task UpdateCompanyCommandHandler_Handle_WhenCompanyIdNotExists_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = CompanyMessages.CompanyIdNotExists;

            var request = new UpdateCompanyCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Company",
                Description = "Updated Description",
                Email = "updated@gmail.com"
            };


            _businessRulesMock.Setup(br => br.EnsureCompanyIdExists(request.Id)).ThrowsAsync(new BusinessException(errorMessage));


            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _businessRulesMock.Object.EnsureCompanyIdExists(request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);
 
            _businessRulesMock.Verify(br => br.EnsureCompanyIdExists(request.Id), Times.Once);
        }
        [Fact]
        public async Task UpdateCompanyCommandHandler_Handle_WhenCompanyNameAlreadyExists_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = CompanyMessages.CompanyNameAlreadyExists;

            var request = new UpdateCompanyCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Company",
                Description = "Updated Description",
                Email = "updated@gmail.com"
            };


            _businessRulesMock.Setup(br => br.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).ThrowsAsync(new BusinessException(errorMessage));


            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _businessRulesMock.Object.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            var exception1 = await Assert.ThrowsAsync<BusinessException>(async () => await _businessRulesMock.Object.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id));
        }
        [Fact]
        public async Task UpdateCompanyCommandHandler_Handle_WhenCompanyDetailsUpToDate_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = CompanyMessages.CompanyDetailsUpToDate;

            var request = new UpdateCompanyCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Company",
                Description = "Updated Description",
                Email = "updated@gmail.com"
            };

            _businessRulesMock.Setup(br => br.EnsureCompanyDetailsAreUpdated(request)).ThrowsAsync(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _businessRulesMock.Object.EnsureCompanyDetailsAreUpdated(request));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _businessRulesMock.Verify(br => br.EnsureCompanyDetailsAreUpdated(request), Times.Once);
        }
    }
}
