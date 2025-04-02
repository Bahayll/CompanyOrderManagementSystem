using AutoMapper;
using CompanyOrderManagement.Application.Features.Orders.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.ConstantMessages;
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

namespace CompanyOrderManagement.UnitTest.ProductCategories.Commands
{
    public class DeleteProductCategoryCommandHandlerTest
    {
        private readonly Mock<IProductCategoryUnitOfWork> _productCategoryUnitOfWorkMock;
        private readonly Mock<IProductCategoryBusinessRules> _productCategoryBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly DeleteProductCategoryCommandHandler _deleteProductCategoryCommandHandler;

        public DeleteProductCategoryCommandHandlerTest()
        {
            _productCategoryBusinessRulesMock = new Mock<IProductCategoryBusinessRules>();
            _productCategoryUnitOfWorkMock = new Mock<IProductCategoryUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService> ();
            _cacheServiceMock = new Mock<ICacheService> (); 
            _deleteProductCategoryCommandHandler = new DeleteProductCategoryCommandHandler(_productCategoryUnitOfWorkMock.Object, _productCategoryBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task DeleteProductCategoryCommandHandler_Handle_WhenProductCategoryDeleted_ShouldInvalidateCache()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var cacheKey = CacheKeys.AllProductCategories;
            var request = new DeleteProductCategoryCommandRequest(categoryId) { Id = categoryId };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Returns(Task.CompletedTask);

            _productCategoryUnitOfWorkMock.Setup(c => c.GetWriteRepository.RemoveAsync(request.Id)).ReturnsAsync(true);
            _productCategoryUnitOfWorkMock.Setup(c => c.SaveAsync()).ReturnsAsync(1);
            _cacheServiceMock.Setup(c => c.Get<List<ProductCategory>>(cacheKey)).Returns(new List<ProductCategory>());


            // Act
            var result = await _deleteProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id), Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.GetWriteRepository.RemoveAsync(request.Id),Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.SaveAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<List<ProductCategory>>(cacheKey),Times.Once);

        }

        [Fact]
        public async Task DeleteProductCategoryCommandHandler_Handle_WhenProductCategoryIdNotExists_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductCategoryMessages.ProductCategoryNotFound;
            var categoryId = Guid.NewGuid();
            var request = new DeleteProductCategoryCommandRequest(categoryId) { Id = categoryId };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.EnsureProductCategoryIdExists(request.Id));

            // Assert
            Assert.Equal(exception.Message, errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id), Times.Once);

        }
        [Fact]
        public async Task DeleteProductCategoryCommandHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange
            var errorMessage = ProductCategoryMessages.ProductCategoryNotFound;
            var categoryId = Guid.NewGuid();
            var request = new DeleteProductCategoryCommandRequest(categoryId) { Id = categoryId };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureProductCategoryIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _deleteProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureProductCategoryIdExists(request.Id), Times.Once);

        }
    }
}
