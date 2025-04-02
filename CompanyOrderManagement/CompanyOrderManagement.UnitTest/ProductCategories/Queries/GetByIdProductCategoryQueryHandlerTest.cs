using AutoMapper;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.ConstantMessages;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyOrderManagement.UnitTest.ProductCategories.Queries
{
    public class GetByIdProductCategoryQueryHandlerTest
    {
        private readonly Mock<IProductCategoryUnitOfWork> _productCategoryUnitOfWorkMock;
        private readonly Mock<IProductCategoryBusinessRules> _productCategoryBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly GetByIdProductCategoryQueryHandler _getByIdProductCategoryQueryHandler;

        public GetByIdProductCategoryQueryHandlerTest()
        {
            _productCategoryBusinessRulesMock = new Mock<IProductCategoryBusinessRules>();
            _productCategoryUnitOfWorkMock = new Mock<IProductCategoryUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService> ();
            _cacheServiceMock = new Mock<ICacheService> (); 
            _getByIdProductCategoryQueryHandler = new GetByIdProductCategoryQueryHandler(_productCategoryUnitOfWorkMock.Object, _productCategoryBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }


        [Fact]
        public async Task GetByIdProductCategoryQueryHandler_Handle_WhenCacheMiss_ShouldFetchFromDatabaseAndUpdateCache()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var cacheKey = CacheKeys.ProductCategoryById(productCategoryId);
            var request = new GetByIdProductCategoryQueryRequest(productCategoryId) { Id = productCategoryId };
            var productCategory = new ProductCategory
            {
                Id = request.Id,
                Name = "Category 1",
                Description = "Description 1",
               
            };

            var response = new GetByIdProductCategoryQueryResponse
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
                CreatedDate = productCategory.CreatedDate,
                LastUpdatedDate = productCategory.LastUpdatedDate
                
            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(productCategoryId)).Returns(Task.CompletedTask);
            _cacheServiceMock.Setup(c => c.Get<ProductCategory>(cacheKey)).Returns((ProductCategory)null);
            _productCategoryUnitOfWorkMock.Setup(c => c.GetReadRepository.GetByIdAsync(productCategoryId, It.IsAny<bool>())).ReturnsAsync(productCategory);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), productCategory, It.IsAny<TimeSpan>(), null));
            _mapperMock.Setup(c => c.Map<GetByIdProductCategoryQueryResponse>(productCategory)).Returns(response);


            // Act

            var result = await _getByIdProductCategoryQueryHandler.Handle(request, CancellationToken.None);

            // Arrange

            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(productCategoryId),Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.GetReadRepository.GetByIdAsync(productCategoryId, It.IsAny<bool>()),Times.Once);
            _mapperMock.Verify(c => c.Map<GetByIdProductCategoryQueryResponse>(productCategory), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<ProductCategory>(cacheKey),Times.Once);
            _mapperMock.Verify(c => c.Map<GetByIdProductCategoryQueryResponse>(productCategory),Times.Once);

        }
        [Fact]
        public async Task GetByIdProductCategoryQueryHandler_Handle_WhenCacheHit_ShouldReturnCachedProductCategory()
        {
            // Arrange
            var productCategoryId = Guid.NewGuid();
            var cacheKey = CacheKeys.ProductCategoryById(productCategoryId);
            var request = new GetByIdProductCategoryQueryRequest(productCategoryId) { Id = productCategoryId };
            var productCategory = new ProductCategory
            {
                Id = request.Id,
                Name = "Category 1",
                Description = "Description 1",

            };

            var response = new GetByIdProductCategoryQueryResponse
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description,
                CreatedDate = productCategory.CreatedDate,
                LastUpdatedDate = productCategory.LastUpdatedDate

            };

            
            _cacheServiceMock.Setup(c => c.Get<ProductCategory>(cacheKey)).Returns(productCategory);
          
            _mapperMock.Setup(c => c.Map<GetByIdProductCategoryQueryResponse>(productCategory)).Returns(response);


            // Act

            var result = await _getByIdProductCategoryQueryHandler.Handle(request, CancellationToken.None);

            // Arrange

            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _cacheServiceMock.Verify(c => c.Get<ProductCategory>(cacheKey),Times.Once);

            _mapperMock.Verify(c => c.Map<GetByIdProductCategoryQueryResponse>(productCategory),Times.Once);

        }
        [Fact]
        public async Task GetByIdProductCategoryQueryHandler_Handle_WhenProductCategoryIdNotExist_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductCategoryMessages.ProductCategoryNotFound;
            var productCategoryId = Guid.NewGuid();
            var request = new GetByIdProductCategoryQueryRequest(productCategoryId) { Id = productCategoryId };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.EnsureProductCategoryIdExists(request.Id));


            // Assert
            Assert.Equal(exception.Message, errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id), Times.Once);
        }
        [Fact]
        public async Task GetByIdProductCategoryQueryHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange
            var errorMessage = ProductCategoryMessages.ProductCategoryNotFound;
            var productCategoryId = Guid.NewGuid();
            var request = new GetByIdProductCategoryQueryRequest(productCategoryId) { Id = productCategoryId };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _getByIdProductCategoryQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id), Times.Once);
        }


    }
    }
