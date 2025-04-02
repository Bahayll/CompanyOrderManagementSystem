using AutoMapper;
using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Rules.Products;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Products.Commands
{
    public class CreateProductCommandHandlerTest
    {
        private readonly Mock<IProductBusinessRules> _productBusinessRulesMock;
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly CreateProductCommandHandler _createProductCommandHandler;

        public CreateProductCommandHandlerTest()
        {
            _productBusinessRulesMock = new Mock<IProductBusinessRules>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _createProductCommandHandler = new CreateProductCommandHandler(_productUnitOfWorkMock.Object, _productBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }
        [Fact]
        public async Task CreateProductCommandHandler_Handle_WhenProductCreatedSuccessfully_ShouldReturnCreatedProduct()
        {
            // Arrange
            var request = new CreateProductCommandRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                CompanyId = request.CompanyId,
                ProductCategoryId = request.ProductCategoryId
            };

            var response = new CreateProductCommandResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                CompanyId = product.CompanyId,
                ProductCategoryId = product.ProductCategoryId
            };

            _productBusinessRulesMock.Setup(p => p.EnsureUniqueProductNameAsync(request.Name)).Returns(Task.CompletedTask);
            _productBusinessRulesMock.Setup(p => p.EnsureCompanyExistsAsync(request.CompanyId)).Returns(Task.CompletedTask);
            _productBusinessRulesMock.Setup(p => p.EnsureCategoryExistsAsync(request.ProductCategoryId)).Returns(Task.CompletedTask);

            _mapperMock.Setup(p => p.Map<Product>(request)).Returns(product);
            _productUnitOfWorkMock.Setup(p => p.GetWriteRepository.AddAsync(product)).Returns(Task.FromResult(true));
            _productUnitOfWorkMock.Setup(p => p.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(p => p.Map<CreateProductCommandResponse>(product)).Returns(response);

            // Act
            var result = await _createProductCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.Equal(response, result.Data);

            _productBusinessRulesMock.Verify(p => p.EnsureUniqueProductNameAsync(request.Name), Times.Once);
            _productBusinessRulesMock.Verify(p => p.EnsureCompanyExistsAsync(request.CompanyId), Times.Once);
            _productBusinessRulesMock.Verify(p => p.EnsureCategoryExistsAsync(request.ProductCategoryId), Times.Once);
            _mapperMock.Verify(p => p.Map<Product>(request), Times.Once);
            _productUnitOfWorkMock.Verify(p => p.GetWriteRepository.AddAsync(product), Times.Once);
            _productUnitOfWorkMock.Verify(p => p.SaveAsync(), Times.Once);
            _mapperMock.Verify(p => p.Map<CreateProductCommandResponse>(product), Times.Once);
        }

        [Fact]
        public async Task CreateProductCommandHandler_Handle_WhenProductNameExists_ShouldThrowBusinessException()
        {

            // Arrange
            var errorMessage = ProductMessages.ProductNameExists;

            var request = new CreateProductCommandRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.EnsureUniqueProductNameAsync(request.Name)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureUniqueProductNameAsync(request.Name));

            // Assert
            Assert.Equal(errorMessage, exception.Message);
 
            _productBusinessRulesMock.Verify(p => p.EnsureUniqueProductNameAsync(request.Name),Times.Once);

        }
        [Fact]
        public async Task CreateProductCommandHandler_Handle_WhenCompanyNotFound_ShouldThrowBusinessException()
        {

            // Arrange
            var errorMessage = ProductMessages.CompanyNotFound;
            var request = new CreateProductCommandRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.EnsureCompanyExistsAsync(request.CompanyId)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureCompanyExistsAsync(request.CompanyId));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureCompanyExistsAsync(request.CompanyId), Times.Once);
        }
        [Fact]
        public async Task CreateProductCommandHandler_Handle_WhenProductCategoryNotFound_ShouldThrowBusinessException()
        {

            // Arrange
            var errorMessage = ProductMessages.ProductCategoryNotFound;
            var request = new CreateProductCommandRequest
            {
                Name = "Test Product",
                Description = "Test Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.EnsureCategoryExistsAsync(request.ProductCategoryId)).Throws(new BusinessException(errorMessage));
             
            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureCategoryExistsAsync(request.ProductCategoryId));
            
            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureCategoryExistsAsync(request.ProductCategoryId), Times.Once);
        }
    }
}
