using AutoMapper;
using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.Commands.Update;
using CompanyOrderManagement.Application.Features.Products.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Products;
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

namespace CompanyOrderManagement.UnitTest.Products.Commands
{
    public class UpdateProductCommandHandlerTest
    {
        private readonly Mock<IProductBusinessRules> _productBusinessRulesMock;
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly UpdateProductCommandHandler _updateProductCommandHandler;

        public UpdateProductCommandHandlerTest()
        {
            _productBusinessRulesMock = new Mock<IProductBusinessRules>();
            _mapperMock = new Mock<IMapper>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _loggerServiceMock = new Mock<ILoggerService> ();

            _updateProductCommandHandler = new UpdateProductCommandHandler(_productUnitOfWorkMock.Object, _productBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }
        [Fact]
        public async Task UpdateProductCommandHandler_Handle_WhenProductUpdatedSuccessfully_ShouldReturnUpdatedProduct()
        { 

            // Arrange
            var request = new UpdateProductCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Stock = 15,
                Price = 200,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            var product = new Product
            {
                Id = request.Id,
                Name = "Product",
                Description = "Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            var response = new UpdateProductCommandResponse
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Stock = request.Stock,
                Price = request.Price,
                CompanyId = request.CompanyId,
                ProductCategoryId = request.ProductCategoryId,
                CreatedDate = product.CreatedDate,
                LastUpdatedDate = DateTime.UtcNow
            };


            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Returns(Task.CompletedTask);
            _productBusinessRulesMock.Setup(p => p.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name,request.Id)).Returns(Task.CompletedTask);
            _productBusinessRulesMock.Setup(p => p.EnsureProductDetailsAreUpdated(request)).Returns(Task.CompletedTask);

            _productUnitOfWorkMock.Setup(p => p.GetReadRepository.GetByIdAsync(request.Id,It.IsAny<bool>())).ReturnsAsync(product);
            _mapperMock.Setup(p => p.Map(request,product)).Returns(It.IsAny<Product>);
            _productUnitOfWorkMock.Setup(p => p.GetWriteRepository.Update(product)).Returns(true);
            _productUnitOfWorkMock.Setup(p => p.SaveAsync()).ReturnsAsync(1);
            _mapperMock.Setup(p => p.Map<UpdateProductCommandResponse>(product)).Returns(response);

            // Act
            var result = await _updateProductCommandHandler.Handle(request,CancellationToken.None);

            // Assert

            Assert.Equal(response,result.Data);
            Assert.Equal(ResponseType.Success,result.ResponseType);


            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id),Times.Once);
            _productBusinessRulesMock.Verify(p => p.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id), Times.Once);
            _productBusinessRulesMock.Verify(p => p.EnsureProductDetailsAreUpdated(request), Times.Once);

            _productUnitOfWorkMock.Verify(p => p.GetReadRepository.GetByIdAsync(request.Id, It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(p => p.Map(request, product), Times.Once);
            _productUnitOfWorkMock.Verify(p => p.GetWriteRepository.Update(product), Times.Once);
            _productUnitOfWorkMock.Verify(p => p.SaveAsync(), Times.Once);
            _mapperMock.Verify(p => p.Map<UpdateProductCommandResponse>(product), Times.Once);

        }
        
        [Fact]
        public async Task UpdateProductCommandHandler_Handle_WhenProductIdDoesNotExist_ShoulThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductNotFound;

            var request = new UpdateProductCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = " Product",
                Description = " Description",
                Stock = 15,
                Price = 200,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).ThrowsAsync(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureProductIdExists(request.Id));
           
            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id), Times.Once);


        }
        public async Task UpdateProductCommandHandler_Handle_WhenProductDetailsAreUpToDate_ShoulThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductDetailsUpToDate;
            var request = new UpdateProductCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = " Product",
                Description = " Description",
                Stock = 15,
                Price = 200,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.EnsureProductDetailsAreUpdated(request)).ThrowsAsync(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureProductDetailsAreUpdated(request));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureProductDetailsAreUpdated(request), Times.Once);

        }
        [Fact]
        public async Task UpdateProductCommandHandler_Handle_WhenProductNameIsDuplicated_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductAlreadyExists;
            var request = new UpdateProductCommandRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Stock = 15,
                Price = 200,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            _productBusinessRulesMock.Setup(p => p.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id)).ThrowsAsync(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id), Times.Once);
        }
    }
}
