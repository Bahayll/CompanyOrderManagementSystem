using AutoMapper;
using CompanyOrderManagement.Application.Features.Products.ConstantMessages;
using CompanyOrderManagement.Application.Features.Products.Queries.GetById;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
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

namespace CompanyOrderManagement.UnitTest.Products.Queries
{
    public class GetByIdProductQueryHandlerTest
    {

        private readonly Mock<IProductBusinessRules> _productBusinessRulesMock;
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly GetByIdProductQueryHandler _getByIdProductQueryHandler;

        public GetByIdProductQueryHandlerTest()
        {
            _productBusinessRulesMock = new Mock<IProductBusinessRules>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _getByIdProductQueryHandler = new GetByIdProductQueryHandler(_productUnitOfWorkMock.Object, _productBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }
        [Fact]
        public async Task GetByIdProductQueryHandler_Handle_WhenProductExists_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new GetByIdProductQueryRequest(productId) { Id = productId };
            var product = new Product
            {
                Id = request.Id,
                Name = "Test Product",
                Description = "Test Description",
                Stock = 10,
                Price = 100,
                CompanyId = Guid.NewGuid(),
                ProductCategoryId = Guid.NewGuid()
            };

            var response = new GetByIdProductQueryResponse
            {
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                Price = product.Price,
                CreatedDate = product.CreatedDate,
                LastUpdatedDate = product.LastUpdatedDate.GetValueOrDefault(),
                CompanyId = product.CompanyId,
                ProductCategoryId = product.ProductCategoryId
            };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Returns(Task.CompletedTask);

            _productUnitOfWorkMock.Setup(p => p.GetReadRepository.GetByIdAsync(request.Id,It.IsAny<bool>())).ReturnsAsync(product);
            _mapperMock.Setup(p => p.Map<GetByIdProductQueryResponse>(product)).Returns(response);

            // Act 

            var result = await _getByIdProductQueryHandler.Handle(request,CancellationToken.None);

            // Assert
            Assert.Equal(response,result.Data);
            Assert.Equal(ResponseType.Success,result.ResponseType);

            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id),Times.Once);
                
            _productUnitOfWorkMock.Verify(p => p.GetReadRepository.GetByIdAsync(request.Id, It.IsAny<bool>()),Times.Once);
            _mapperMock.Verify(p => p.Map<GetByIdProductQueryResponse>(product),Times.Once);
        }

        [Fact]
        public async Task GetByIdProductQueryHandler_Handle_WhenProductIdNotExist_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductNotFound;
            var productId = Guid.NewGuid();
            var request = new GetByIdProductQueryRequest(productId) { Id = productId };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureProductIdExists(request.Id));

            // Assert 
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id), Times.Once);

        }
        [Fact]
        public async Task GetByIdProductQueryHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductNotFound;
            var productId = Guid.NewGuid();
            var request = new GetByIdProductQueryRequest(productId) { Id = productId };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _getByIdProductQueryHandler.Handle(request, CancellationToken.None);

            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);


            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id), Times.Once);

        }
    }
}
