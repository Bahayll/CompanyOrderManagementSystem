using AutoMapper;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;
using CompanyOrderManagement.Application.Features.ProductCategories.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CompanyOrderManagement.UnitTest.ProductCategories.Commands
{
    public class UpdateProductCategoryCommandHandlerTest
    {
        private readonly Mock<IProductCategoryUnitOfWork> _productCategoryUnitOfWorkMock;
        private readonly Mock<IProductCategoryBusinessRules> _productCategoryBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly UpdateProductCategoryCommandHandler _updateProductCategoryCommandHandler;

        public UpdateProductCategoryCommandHandlerTest()
        {
            _productCategoryBusinessRulesMock = new Mock<IProductCategoryBusinessRules>();
            _productCategoryUnitOfWorkMock = new Mock<IProductCategoryUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock =new Mock<ILoggerService> ();
            _cacheServiceMock = new Mock<ICacheService> ();
            _updateProductCategoryCommandHandler = new UpdateProductCategoryCommandHandler(_productCategoryUnitOfWorkMock.Object, _productCategoryBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task UpdateProductCategoryCommandHandler_Handle_ShouldReturnUpdatedProductCategory()
        {

            // Arrange 
            var cacheKey = CacheKeys.AllProductCategories;
            var request = new UpdateProductCategoryCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Description"
            };

            var productCategory = new ProductCategory
            {
                Id = request.Id,
                Name = "Category",
                Description = "Description"
            };

            var response = new UpdateProductCategoryCommandResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CreatedDate = productCategory.CreatedDate,
                LastUpdatedDate=DateTime.UtcNow,
            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Returns(Task.CompletedTask);
            _productCategoryBusinessRulesMock.Setup(c => c.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).Returns(Task.CompletedTask);
            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductDetailsAreUpdated(request));

            _productCategoryUnitOfWorkMock.Setup(c => c.GetReadRepository.GetByIdAsync(request.Id, It.IsAny<bool>())).ReturnsAsync(productCategory);
            _mapperMock.Setup(c => c.Map(request,productCategory)).Returns(It.IsAny<ProductCategory>);
            _productCategoryUnitOfWorkMock.Setup(c => c.GetWriteRepository.Update(productCategory)).Returns(true);
            _productCategoryUnitOfWorkMock.Setup(c => c.SaveAsync()).ReturnsAsync(1);


            _mapperMock.Setup(c => c.Map<UpdateProductCategoryCommandResponse>(request)).Returns(response);

            // Act
            var result = await _updateProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success,result.ResponseType);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id),Times.Once);
            _productCategoryBusinessRulesMock.Verify(c => c.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id), Times.Once);
            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductDetailsAreUpdated(request),Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.GetReadRepository.GetByIdAsync(request.Id,It.IsAny<bool>()),Times.Once);
            _mapperMock.Verify(c => c.Map(request,productCategory),Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.GetWriteRepository.Update(productCategory),Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.SaveAsync(),Times.Once);
            _mapperMock.Verify(c => c.Map<UpdateProductCategoryCommandResponse>(request),Times.Once);
                
        }
        [Fact]
        public async Task UpdateProductCategoryCommandHandler_Handle_WhenProductCategoryIsUpdated_ShouldUpdateCache()
        {

            // Arrange 
            var cacheKey = CacheKeys.AllProductCategories;
            var request = new UpdateProductCategoryCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Description"
            };

            var productCategory = new ProductCategory
            {
                Id = request.Id,
                Name = "Category",
                Description = "Description"
            };

            var response = new UpdateProductCategoryCommandResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                CreatedDate = productCategory.CreatedDate,
                LastUpdatedDate = DateTime.UtcNow,
            };
            var cachedProductCategories = new List<ProductCategory> { productCategory };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Returns(Task.CompletedTask);
            _productCategoryBusinessRulesMock.Setup(c => c.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).Returns(Task.CompletedTask);
            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductDetailsAreUpdated(request));

            _productCategoryUnitOfWorkMock.Setup(c => c.GetReadRepository.GetByIdAsync(request.Id, It.IsAny<bool>())).ReturnsAsync(productCategory);
            _mapperMock.Setup(c => c.Map(request, productCategory)).Returns(It.IsAny<ProductCategory>);
            _productCategoryUnitOfWorkMock.Setup(c => c.GetWriteRepository.Update(productCategory)).Returns(true);
            _productCategoryUnitOfWorkMock.Setup(c => c.SaveAsync()).ReturnsAsync(1);
            _cacheServiceMock.Setup(c => c.Get<List<ProductCategory>>(cacheKey)).Returns(cachedProductCategories);
            _cacheServiceMock.Setup(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<ProductCategory>>(), It.IsAny<TimeSpan>(), null));

            _mapperMock.Setup(c => c.Map<UpdateProductCategoryCommandResponse>(request)).Returns(response);

            // Act
            var result = await _updateProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _cacheServiceMock.Verify(c => c.Get<List<ProductCategory>>(cacheKey),Times.Once);
            _cacheServiceMock.Verify(c => c.Set(It.Is<string>(key => key == cacheKey), It.IsAny<List<ProductCategory>>(), It.IsAny<TimeSpan>(), null),Times.Once);


        }
        [Fact]
        public async Task UpdateProductCategoryCommandHandler_Handle_WhenProductCategoryIdNotFound_ShouldThrowBusinessException()
        {


            // Arrange 
            var errorMessage = ProductCategoryMessages.ProductCategoryNotFound;

            var request = new UpdateProductCategoryCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Description"
            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.EnsureProductCategoryIdExists(request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id),Times.Once);

        }
        [Fact]
        public async Task UpdateProductCategoryCommandHandler_Handle_WhenCategoryNameAlreadyExists_ShouldThrowBusinessException()
        {
            // Arrange 
            var errorMessage = ProductCategoryMessages.CategoryNameExists;

            var request = new UpdateProductCategoryCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Description"
            };


            _productCategoryBusinessRulesMock.Setup(c => c.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productCategoryBusinessRulesMock.Verify(c => c.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id), Times.Once);
        }
        [Fact]
        public async Task UpdateProductCategoryCommandHandler_Handle_WhenProductCategoryDetailsAreUpToDate_ShouldThrowBusinessException()
        {
            // Arrange 
            var errorMessage = ProductCategoryMessages.ProductCategoryDetailsUpToDate;

            var request = new UpdateProductCategoryCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Category",
                Description = "Updated Description"
            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductDetailsAreUpdated(request)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.EnsureProductDetailsAreUpdated(request));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductDetailsAreUpdated(request), Times.Once);
        }
    }
}
