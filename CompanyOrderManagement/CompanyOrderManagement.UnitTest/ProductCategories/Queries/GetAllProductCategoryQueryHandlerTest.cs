using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetAll;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.ProductCategories.Queries
{
    public class GetAllProductCategoryQueryHandlerTest
    {
        private readonly Mock<IProductCategoryUnitOfWork> _productCategoryUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly GetAllProductCategoryQueryHandler _getAllProductCategoryQueryHandler;

        public GetAllProductCategoryQueryHandlerTest()
        {
         
            _productCategoryUnitOfWorkMock = new Mock<IProductCategoryUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService> ();
            _cacheServiceMock = new Mock<ICacheService> ();
            _getAllProductCategoryQueryHandler = new GetAllProductCategoryQueryHandler(_productCategoryUnitOfWorkMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetAllProductCategoryQueryHandler_Handle_WhenCacheMiss_ShouldFetchFromDatabaseAndUpdateCache()
        {
            // Arrange
            var cacheKey = CacheKeys.GetAllProductCategories;
            var request = new GetAllProductCategoryQueryRequest();
            var productCategories = new List<ProductCategory>
            {
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Category 1",
                    Description = "Description 1"
                },
                new ProductCategory
                {
                    Id = Guid.NewGuid(),
                    Name = "Category 2",
                    Description = "Description 2"
                }
            };

            var productCategoryResponse = productCategories.Select(c => new GetAllProductCategoryQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
                LastUpdatedDate = c.LastUpdatedDate
            }).ToList();

            _cacheServiceMock.Setup(c => c.Get<List<GetAllProductCategoryQueryResponse>>(cacheKey)).Returns((List<GetAllProductCategoryQueryResponse>)null);
            _productCategoryUnitOfWorkMock.Setup(c => c.GetReadRepository.GetAll(It.IsAny<bool>())).Returns(productCategories.AsQueryable());
            _mapperMock.Setup(c => c.Map<List<GetAllProductCategoryQueryResponse>>(productCategories)).Returns(productCategoryResponse);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<GetAllProductCategoryQueryResponse>>(), It.IsAny<TimeSpan>(), null));


            // Act 
            var result = await _getAllProductCategoryQueryHandler.Handle(request, CancellationToken.None);


            // Assert
            Assert.Equal(productCategoryResponse, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _productCategoryUnitOfWorkMock.Verify(c => c.GetReadRepository.GetAll(It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(c => c.Map<List<GetAllProductCategoryQueryResponse>>(productCategories), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<List<GetAllProductCategoryQueryResponse>>(cacheKey),Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<GetAllProductCategoryQueryResponse>>(), It.IsAny<TimeSpan>(), null),Times.Once);

        }
        [Fact]
        public async Task GetAllProductCategoryQueryHandler_Handle_WhenCacheHit_ShouldReturnCachedProductCategories()
        {
            // Arrange
            var cacheKey = CacheKeys.GetAllProductCategories;
            var request = new GetAllProductCategoryQueryRequest();
            var productCategoriesFromCache = new List<GetAllProductCategoryQueryResponse>
            {
                new GetAllProductCategoryQueryResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Category 1",
                    Description = "Description 1"
                },
                new GetAllProductCategoryQueryResponse
                {
                    Id = Guid.NewGuid(),
                    Name = "Category 2",
                    Description = "Description 2"
                }
            };

            _cacheServiceMock.Setup(c => c.Get<List<GetAllProductCategoryQueryResponse>>(cacheKey)).Returns(productCategoriesFromCache);
           


            // Act 
            var result = await _getAllProductCategoryQueryHandler.Handle(request, CancellationToken.None);


            // Assert
            Assert.Equal(productCategoriesFromCache, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);
            
            _cacheServiceMock.Verify(c => c.Get<List<GetAllProductCategoryQueryResponse>>(cacheKey));



        }
    }
}
