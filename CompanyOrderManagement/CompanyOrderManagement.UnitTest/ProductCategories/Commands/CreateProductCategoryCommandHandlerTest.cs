using AutoMapper;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
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
using System.Xml.Linq;
using Xunit;

namespace CompanyOrderManagement.UnitTest.ProductCategories.Commands
{
    public class CreateProductCategoryCommandHandlerTest
    {
        private readonly Mock<IProductCategoryUnitOfWork> _productCategoryUnitOfWorkMock;
        private readonly Mock<IProductCategoryBusinessRules> _productCategoryBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly CreateProductCategoryCommandHandler _createProductCategoryCommandHandler;

        public CreateProductCategoryCommandHandlerTest()
        {
            _productCategoryBusinessRulesMock = new Mock<IProductCategoryBusinessRules>();
            _productCategoryUnitOfWorkMock = new Mock<IProductCategoryUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _createProductCategoryCommandHandler = new CreateProductCategoryCommandHandler(_productCategoryUnitOfWorkMock.Object, _productCategoryBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object,_cacheServiceMock.Object);
        }

        [Fact]
        public async Task CreateProductCategoryCommandHandler_Handle_WhenProductCategoryCreatedSuccessfully_ShouldReturnsCreatedProductCategory()
        {
            // Arrange
            var cacheKey = CacheKeys.AllProductCategories;
            var request = new CreateProductCategoryCommandRequest
            {
                Name = "Test Category",
                Description = "Test Description"

            };

            var productCategory = new ProductCategory
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            var response = new CreateProductCategoryCommandResponse
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                Description = productCategory.Description
            };


            _productCategoryBusinessRulesMock.Setup(c => c.EnsureUniqueCategoryNameAsync(request.Name)).Returns(Task.CompletedTask);
            _mapperMock.Setup(c => c.Map<ProductCategory>(request)).Returns(productCategory);
            _productCategoryUnitOfWorkMock.Setup(c => c.GetWriteRepository.AddAsync(productCategory)).ReturnsAsync(true);
            _productCategoryUnitOfWorkMock.Setup(c => c.SaveAsync()).ReturnsAsync(1);
            _cacheServiceMock.Setup(c => c.Get<List<ProductCategory>>(cacheKey)).Returns(new List<ProductCategory>());
            _cacheServiceMock.Setup(c => c.Set(cacheKey, It.IsAny<List<ProductCategory>>(), It.IsAny<TimeSpan>(), null));
            _mapperMock.Setup(c => c.Map<CreateProductCategoryCommandResponse>(productCategory)).Returns(response);



            // Act

            var result = await _createProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert

            Assert.Equal(response.Id, result.Data.Id);
            Assert.Equal(response.Name, result.Data.Name);
            Assert.Equal(response.Description, result.Data.Description);
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);


            _productCategoryBusinessRulesMock.Verify(c => c.EnsureUniqueCategoryNameAsync(request.Name), Times.Once);
            _mapperMock.Verify(c => c.Map<ProductCategory>(request), Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.GetWriteRepository.AddAsync(productCategory), Times.Once);
            _productCategoryUnitOfWorkMock.Verify(c => c.SaveAsync(), Times.Once);
            _cacheServiceMock.Verify(c => c.Get<List<ProductCategory>>(cacheKey),Times.Once);
            _cacheServiceMock.Verify(c => c.Set(cacheKey, It.IsAny<List<ProductCategory>>(), It.IsAny<TimeSpan>(), null),Times.Once);


        }
        [Fact]
        public async Task CreateProductCategoryCommandHandler_Handle_WhenUniqueCategoryNameCheckFails_ShouldThrowBusinessException()
        {
            // Arrange 

            var errorMessage = ProductCategoryMessages.CategoryNameExists;

            var request = new CreateProductCategoryCommandRequest
            {
                Name = "Test Category",
                Description = "Test Description"

            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureUniqueCategoryNameAsync(request.Name)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productCategoryBusinessRulesMock.Object.EnsureUniqueCategoryNameAsync(request.Name));
            // Assert

            Assert.Equal(exception.Message, errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureUniqueCategoryNameAsync(request.Name), Times.Once);
        }
        [Fact]
        public async Task CreateProductCategoryCommandHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange 

            var errorMessage = ProductCategoryMessages.CategoryNameExists;

            var request = new CreateProductCategoryCommandRequest
            {
                Name = "Test Category",
                Description = "Test Description"

            };

            _productCategoryBusinessRulesMock.Setup(c => c.EnsureUniqueCategoryNameAsync(request.Name)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _createProductCategoryCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _productCategoryBusinessRulesMock.Verify(c => c.EnsureUniqueCategoryNameAsync(request.Name), Times.Once);
        }

    }
}
