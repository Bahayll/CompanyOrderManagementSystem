using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
using CompanyOrderManagement.Application.Features.Companies.ConstantMessages;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetById;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Companies.Queries
{
    public class GetByIdCompanyQueryHandlerTest
    {
        private readonly Mock<ICompanyUnitOfWork> _companyUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICompanyBusinessRules> _companyBusinessRulesMock; 
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly GetByIdCompanyQueryHandler _getByIdCompanyQueryHandler;

        public GetByIdCompanyQueryHandlerTest()
        {
            _companyUnitOfWorkMock = new Mock<ICompanyUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<ICacheService>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _companyBusinessRulesMock = new Mock<ICompanyBusinessRules>();
            _getByIdCompanyQueryHandler = new GetByIdCompanyQueryHandler(_companyUnitOfWorkMock.Object, _companyBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetByIdCompanyQueryHandler_Handle_WhenCacheMiss_ShouldFetchFromDatabaseAndUpdateCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cacheKey = CacheKeys.CompanyById(id);
            var request = new GetByIdCompanyQueryRequest(id) { Id = id };
            var company = new Company
            {
                Id = request.Id,
                Name = "Company",
                Description = "Description",
                Email = "company@gmail.com"
            };
            var response = new GetByIdCompanyQueryResponse
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Email = company.Email,
                CreatedDate = company.CreatedDate,
                LastUpdatedDate = company.LastUpdatedDate
            };
            
            _companyBusinessRulesMock.Setup(cg => cg.EnsureCompanyIdExists(request.Id)).Returns(Task.CompletedTask);

            _cacheServiceMock.Setup(c => c.Get<Company>(cacheKey)).Returns((Company)null);
            _companyUnitOfWorkMock.Setup(cg => cg.GetReadRepository.GetByIdAsync(request.Id, true)).ReturnsAsync(company);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), company, It.IsAny<TimeSpan>(), null));
            _mapperMock.Setup(cg => cg.Map<GetByIdCompanyQueryResponse>(company)).Returns(response);


            // Act
            var result = await _getByIdCompanyQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _companyBusinessRulesMock.Verify(cg => cg.EnsureCompanyIdExists(request.Id), Times.Once);
            _companyUnitOfWorkMock.Verify(cg => cg.GetReadRepository.GetByIdAsync(request.Id, true), Times.Once);
            _mapperMock.Verify(cg => cg.Map<GetByIdCompanyQueryResponse>(company), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<Company>(cacheKey),Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), company, It.IsAny<TimeSpan>(), null),Times.Once);
        }
        [Fact]
        public async Task GetByIdCompanyQueryHandler_Handle_WhenCacheHit_ShouldReturnCachedCompany()
        {
            var id = Guid.NewGuid();
            var cacheKey = CacheKeys.CompanyById(id);
            var request = new GetByIdCompanyQueryRequest(id) { Id = id };
            var company = new Company
            {
                Id = request.Id,
                Name = "Company",
                Description = "Description",
                Email = "company@gmail.com"
            };
            var response = new GetByIdCompanyQueryResponse
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Email = company.Email,
                CreatedDate = company.CreatedDate,
                LastUpdatedDate = company.LastUpdatedDate
            };

            _cacheServiceMock.Setup(c => c.Get<Company>(cacheKey)).Returns(company);
            _mapperMock.Setup(m => m.Map<GetByIdCompanyQueryResponse>(company)).Returns(response);

            // Act
            var result = await _getByIdCompanyQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _cacheServiceMock.Verify(c => c.Get<Company>(cacheKey),Times.Once);
            _mapperMock.Verify(m => m.Map<GetByIdCompanyQueryResponse>(company),Times.Once);
        }
        [Fact]
        public async Task GetByIdCompanyQueryHandler_Handle_WhenCompanyIdNotExists_ShouldThrowBusinessException()
        {
            // Arrange 
            var errorMessage = CompanyMessages.CompanyIdNotExists;
            var id = Guid.NewGuid();
            var request = new GetByIdCompanyQueryRequest(id) { Id = id };

            _companyBusinessRulesMock.Setup(c => c.EnsureCompanyIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _companyBusinessRulesMock.Object.EnsureCompanyIdExists(request.Id));

            // Assert

            Assert.Equal(exception.Message, errorMessage);

            _companyBusinessRulesMock.Verify(c => c.EnsureCompanyIdExists(request.Id), Times.Once);
        }
        [Fact]
        public async Task GetByIdCompanyQueryHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange 
            var errorMessage = CompanyMessages.CompanyIdNotExists;
            var id = Guid.NewGuid();
            var request = new GetByIdCompanyQueryRequest(id) { Id = id };

            _companyBusinessRulesMock.Setup(c => c.EnsureCompanyIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _getByIdCompanyQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _companyBusinessRulesMock.Verify(c => c.EnsureCompanyIdExists(request.Id), Times.Once);
        }


    }
}
