using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Enums;
using CompanyOrderManagement.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Orders.Commands
{
    public class CreateOrderCommandHandlerTest
    {
        private readonly Mock<IOrderUnitOfWork> _orderUnitOfWorkMock;
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IOrderBusinessRules> _orderBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly CreateOrderCommandHandler _createOrderCommandHandler;

        public CreateOrderCommandHandlerTest()
        {
            _orderUnitOfWorkMock = new Mock<IOrderUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _orderBusinessRulesMock = new Mock<IOrderBusinessRules>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _createOrderCommandHandler = new CreateOrderCommandHandler(_orderUnitOfWorkMock.Object, _productUnitOfWorkMock.Object, _orderBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object );
        }

        [Fact]
        public async Task CreateOrderCommandHandler_Handle_WhenOrderCreatedSuccessfully_ShouldReturnCreatedOrder()
        {
            // Arrange
            var request = new CreateOrderCommandRequest
            {
                Name = "Test Order",
                Description = "Test Description",
                Address = "Test Address",
                ProductCount = 2,
                UnitPrice = 100,
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                ProductsId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };
            var products = new List<Product>
            {
                new Product { Id = request.ProductsId.First(), CompanyId = request.CompanyId, Price = 5, Stock = 100,ProductCategoryId=Guid.NewGuid() },
                new Product { Id = request.ProductsId.Last(), CompanyId = request.CompanyId, Price = 5, Stock = 100,ProductCategoryId=Guid.NewGuid() }
            };

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                ProductCount = request.ProductCount,
                UnitPrice = request.UnitPrice,
                UserId = request.UserId,
                CompanyId = request.CompanyId,
                Products = products,
                OrderStatus = OrderStatus.Processing,
            };
           
 
            var response = new CreateOrderCommandResponse
            {

                Id = order.Id,
                Name = order.Name,
                Description = order.Description,
                ProductCount = order.ProductCount,
                UnitPrice = order.UnitPrice,
                TotalPrice = order.UnitPrice * order.ProductCount,
                Address = order.Address,
                UserId = order.UserId,
                CompanyId = order.CompanyId,
                ProductsId = order.Products.Select(p => p.Id).ToList()
            };
          

            _orderBusinessRulesMock.Setup(o => o.EnsureCompanyExistsAsync(request.CompanyId)).Returns(Task.CompletedTask);

            _orderBusinessRulesMock.Setup(o => o.EnsureUserIsRegisteredForOrderAsync(request.UserId)).Returns(Task.CompletedTask);

            _mapperMock.Setup(o => o.Map<Order>(request)).Returns(order);
            _productUnitOfWorkMock.Setup(o => o.GetReadRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns<Guid, bool>((id, includeDeleted) => Task.FromResult(products.FirstOrDefault(p => p.Id == id)));
            _orderUnitOfWorkMock.Setup(o => o.GetWriteRepository.AddAsync(order)).Returns(Task.FromResult(true));

            _orderUnitOfWorkMock.Setup(o => o.SaveAsync()).Returns(Task.FromResult(1));

            _mapperMock.Setup(o => o.Map<CreateOrderCommandResponse>(It.IsAny<Order>())).Returns(response);
                            

            // Act
            var result = await _createOrderCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(order.Id, result.Data.Id);
            Assert.Equal(order.Name, result.Data.Name);
            Assert.Equal(order.Description, result.Data.Description);
            Assert.Equal(order.ProductCount, result.Data.ProductCount);
            Assert.Equal(order.UnitPrice, result.Data.UnitPrice);
            Assert.Equal(order.TotalPrice, result.Data.TotalPrice);

            _orderBusinessRulesMock.Verify(o => o.EnsureCompanyExistsAsync(request.CompanyId), Times.Once);
            _orderBusinessRulesMock.Verify(o => o.EnsureUserIsRegisteredForOrderAsync(request.UserId), Times.Once);
            _productUnitOfWorkMock.Verify(p => p.GetReadRepository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Exactly(request.ProductsId.Count));
            _orderUnitOfWorkMock.Verify(o => o.GetWriteRepository.AddAsync(It.IsAny<Order>()), Times.Once);
            _orderUnitOfWorkMock.Verify(o => o.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task CreateOrderCommandHandler_Handle_WhenUserNotRegistered_ShouldThrowBusinessException()
        {
            var errorMessage = OrderMessages.UserMustBeRegisteredForOrder;

            var request = new CreateOrderCommandRequest
            {
                Name = "Test Order",
                Description = "Test Description",
                Address = "Test Address",
                ProductCount = 2,
                UnitPrice = 100,
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                ProductsId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };  

            _orderBusinessRulesMock.Setup(o => o.EnsureUserIsRegisteredForOrderAsync(request.UserId)).Throws(new BusinessException(errorMessage));

            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _orderBusinessRulesMock.Object.EnsureUserIsRegisteredForOrderAsync(request.UserId));

            Assert.Equal(errorMessage, exception.Message);

            _orderBusinessRulesMock.Verify(o => o.EnsureUserIsRegisteredForOrderAsync(request.UserId),Times.Once);
        }
        [Fact]
        public async Task CreateOrderCommandHandler_Handle_WhenCompanyDoesNotExist_ShouldThrowBusinessException()
        {
            // Arrange
            var errorMessage = OrderMessages.CompanyMustExistForOrder;
            var request = new CreateOrderCommandRequest
            {
                Name = "Test Order",
                Description = "Test Description",
                Address = "Test Address",
                ProductCount = 2,
                UnitPrice = 100,
                UserId = Guid.NewGuid(),
                CompanyId = Guid.NewGuid(),
                ProductsId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            _orderBusinessRulesMock.Setup(o => o.EnsureCompanyExistsAsync(request.CompanyId)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _orderBusinessRulesMock.Object.EnsureCompanyExistsAsync(request.CompanyId));

            // Assert
            Assert.Equal(errorMessage, exception.Message);

            _orderBusinessRulesMock.Verify(o => o.EnsureCompanyExistsAsync(request.CompanyId), Times.Once);
        }

    }
}
