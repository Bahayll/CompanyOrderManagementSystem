using AutoMapper;
using CompanyOrderManagement.Application.Features.Orders.Commands.Delete;
using CompanyOrderManagement.Application.Features.Orders.ConstantMessages;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetById;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Enums;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Orders.Queris
{
    public class GetByIdOrderQueryHandlerTest
    {

        private readonly Mock<IOrderUnitOfWork> _orderUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderBusinessRules> _orderBusinessRulesMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly GetByIdOrderQueryHandler _getByIdOrderQuerydHandler;

        public GetByIdOrderQueryHandlerTest()
        {
            _orderUnitOfWorkMock = new Mock<IOrderUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _orderBusinessRulesMock = new Mock<IOrderBusinessRules>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _getByIdOrderQuerydHandler = new GetByIdOrderQueryHandler(_orderUnitOfWorkMock.Object,_orderBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }


        [Fact]
        public async Task GetByIdOrderQueryHandler_Handle_WhenOrderExists_ShouldReturnOrder()
        {

            // Arrange 
            var orderId = Guid.NewGuid();
            var request = new GetByIdOrderQueryRequest(orderId) { Id = orderId };
            var order = new Order
            {
                Id = request.Id,
                Name = "Test Order",
                Address = "Test Address",
                Description = "Description",
                OrderStatus = OrderStatus.Processing,
                CompanyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ProductCount = 100,
                TotalPrice = 500,
                UnitPrice = 5,

            };

            var response = new GetByIdOrderQueryResponse
            { 
                Name = order.Name,
                Address = order.Address,
                Description = order.Description,
                CreatedDate = order.CreatedDate,
                LastUpdatedDate = order.LastUpdatedDate,
                OrderStatus = order.OrderStatus,
                UnitPrice = order.UnitPrice,
                TotalPrice = order.TotalPrice,
                CompanyId = order.CompanyId,
                ProductCount = order.ProductCount,
                UserId = order.UserId,
                ProductsId = new List<Guid>(),
            };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Returns(Task.CompletedTask);

            _orderUnitOfWorkMock.Setup(o => o.GetReadRepository.GetByIdAsync(request.Id,It.IsAny<bool>())).ReturnsAsync(order);

            _mapperMock.Setup(o => o.Map<GetByIdOrderQueryResponse>(order)).Returns(response);


            // Act
            var result = await _getByIdOrderQuerydHandler.Handle(request,CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);
            Assert.Equal(response, result.Data);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id),Times.Once);
            _orderUnitOfWorkMock.Verify(o => o.GetReadRepository.GetByIdAsync(request.Id, It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(o => o.Map<GetByIdOrderQueryResponse>(order), Times.Once);

        }
        [Fact]
        public async Task GetByIdOrderQueryHandler_Handle_WhenOrderIdNotExists_ShouldThrowException()
        {
            // Arrange
            var errorMessage = OrderMessages.OrderIdNotFound;

            var orderId = Guid.NewGuid();
            var request = new GetByIdOrderQueryRequest(orderId) { Id = orderId };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _orderBusinessRulesMock.Object.EnsureOrderIdExists(request.Id));

            // Assert
            Assert.Equal(exception.Message,errorMessage);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id), Times.Once);


        }
        [Fact]
        public async Task GetByIdOrderQueryHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {
            // Arrange
            var errorMessage = OrderMessages.OrderIdNotFound;

            var orderId = Guid.NewGuid();
            var request = new GetByIdOrderQueryRequest(orderId) { Id = orderId };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _getByIdOrderQuerydHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id), Times.Once);


        }
    }
}
