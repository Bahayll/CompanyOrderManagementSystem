using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetAll;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Companies.Queries
{
    public class GetAllCompanyQueryHandlerTest
    {
        private readonly Mock<ICompanyUnitOfWork> _companyUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly GetAllCompanyQueryHandler _getAllCompanyQueryHandler;

        public GetAllCompanyQueryHandlerTest()
        {
            _companyUnitOfWorkMock = new Mock<ICompanyUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _getAllCompanyQueryHandler = new GetAllCompanyQueryHandler(_companyUnitOfWorkMock.Object,_mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetAllCompanyQueryHandler_Handle_WhenCacheMiss_ShouldFetchFromDatabaseAndUpdateCache()
        {

            // Arrange
            var request = new GetAllCompanyQueryRequest();
            var cacheKey = CacheKeys.GetAllCompanies;
            var companies = new List<Company>
            {
                new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 1",
                    Description = "Description 1",
                    Email = "email1@gmail.com",
                },
                new Company
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 2",
                    Description = "Description 2",
                    Email = "email2@gmail.com",
                }
            };

            var response = companies.Select(c => new GetAllCompanyQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Email = c.Email,
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            _cacheServiceMock.Setup(c => c.Get<List<GetAllCompanyQueryResponse>>(cacheKey)).Returns((List<GetAllCompanyQueryResponse>)null);

            _companyUnitOfWorkMock.Setup(c => c.GetReadRepository.GetAll(It.IsAny<bool>())).Returns(companies.AsQueryable());

            _mapperMock.Setup(c => c.Map<List<GetAllCompanyQueryResponse>>(companies)).Returns(response);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<GetAllCompanyQueryResponse>>(), It.IsAny<TimeSpan>(), null));



            // Act 

            var result = await _getAllCompanyQueryHandler.Handle(request, CancellationToken.None);

            // Assert   

            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);



            _companyUnitOfWorkMock.Verify(c => c.GetReadRepository.GetAll(It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(c => c.Map<List<GetAllCompanyQueryResponse>>(companies), Times.Once);
            _cacheServiceMock.Verify(cc => cc.Get<List<GetAllCompanyQueryResponse>>(It.Is<string>(key => key == cacheKey)), Times.Once);
            _cacheServiceMock.Verify(x => x.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<GetAllCompanyQueryResponse>>(), It.IsAny<TimeSpan>(), null), Times.Once);
        }
        [Fact]
        public async Task GetAllCompanyQueryHandler_Handle_WhenCacheHit_ShouldReturnCachedCompanies()
        {
            // Arrange
            var cacheKey = CacheKeys.GetAllCompanies;
            var request = new GetAllCompanyQueryRequest();
            var companiesFromCache = new List<GetAllCompanyQueryResponse>
            {
                new GetAllCompanyQueryResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 1",
                    Description = "Description 1",
                    Email = "email1@gmail.com"
                },
                new GetAllCompanyQueryResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Company 2",
                    Description = "Description 2",
                    Email = "email2@gmail.com"
                }
            };

            _cacheServiceMock.Setup(c => c.Get<List<GetAllCompanyQueryResponse>>(cacheKey)).Returns(companiesFromCache);

            // Act
            var result = await _getAllCompanyQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(companiesFromCache, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _companyUnitOfWorkMock.Verify(c => c.GetReadRepository.GetAll(It.IsAny<bool>()), Times.Never);
        }


    }
}
