using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.Commands.Delete;
using CompanyOrderManagement.Application.Features.Products.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Products;
using CompanyOrderManagement.Application.Services.Cache;
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
    public class DeleteProductCommandHandlerTest
    {
        
        private readonly Mock<IProductBusinessRules> _productBusinessRulesMock;
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly DeleteProductCommandHandler _deleteProductCommandHandler;

        public DeleteProductCommandHandlerTest()
        {
            _productBusinessRulesMock = new Mock<IProductBusinessRules>();
            _mapperMock = new Mock<IMapper>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _deleteProductCommandHandler = new DeleteProductCommandHandler(_productUnitOfWorkMock.Object, _productBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }
        [Fact]
        public async Task DeleteProductCommandHandler_Handle_WhenProductDeletedSuccessfully_ShouldReturnNoContent()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var request = new DeleteProductCommandRequest(productId) { Id = productId };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Returns(Task.CompletedTask);
            _productUnitOfWorkMock.Setup(p => p.GetWriteRepository.RemoveAsync(request.Id)).ReturnsAsync(true);
            _productUnitOfWorkMock.Setup(p => p.SaveAsync()).ReturnsAsync(1);

            // Act
            var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommandRequest(productId), CancellationToken.None);


            // Assert 
            Assert.Equal(ResponseType.Success,result.ResponseType);

            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id),Times.Once);
            _productUnitOfWorkMock.Verify(p => p.GetWriteRepository.RemoveAsync(request.Id),Times.Once);
            _productUnitOfWorkMock.Verify(p => p.SaveAsync(),Times.Once);


        }
        [Fact]
        public async Task DeleteProductCommandHandler_Handle_WhenProductIdNotExists_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductNotFound;
            var productId = Guid.NewGuid();
            var request = new DeleteProductCommandRequest(productId) { Id = productId };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            //Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRulesMock.Object.EnsureProductIdExists(request.Id));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id),Times.Once);
        }
        [Fact]
        public async Task DeleteProductCommandHandler_Handle_WhenBusinessRuleFails_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = ProductMessages.ProductNotFound;
            var productId = Guid.NewGuid();
            var request = new DeleteProductCommandRequest(productId) { Id = productId };

            _productBusinessRulesMock.Setup(p => p.EnsureProductIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            //Act
            var result = await _deleteProductCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);


            _productBusinessRulesMock.Verify(p => p.EnsureProductIdExists(request.Id), Times.Once);
        }


    }
}
