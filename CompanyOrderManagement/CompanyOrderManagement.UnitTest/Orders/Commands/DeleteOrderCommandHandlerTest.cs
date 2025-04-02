using AutoMapper;
using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Commands.Delete;
using CompanyOrderManagement.Application.Features.Orders.ConstantMessages;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace CompanyOrderManagement.UnitTest.Orders.Commands
{
    public class DeleteOrderCommandHandlerTest
    {
        private readonly Mock<IOrderUnitOfWork> _orderUnitOfWorkMock;
        private readonly Mock<IOrderBusinessRules> _orderBusinessRulesMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly DeleteOrderCommandHandler _deleteOrderCommandHandler;

        public DeleteOrderCommandHandlerTest()
        {
            _orderUnitOfWorkMock = new Mock<IOrderUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _orderBusinessRulesMock = new Mock<IOrderBusinessRules>();
            _loggerServiceMock = new Mock<ILoggerService> ();
            _deleteOrderCommandHandler = new DeleteOrderCommandHandler(_orderUnitOfWorkMock.Object, _orderBusinessRulesMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }

        [Fact]
        public async Task DeleteOrderCommandHandler_Handle_WhenOrderDeletedSuccessfully_ShouldReturnNoContent()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var request = new DeleteOrderCommandRequest(orderId)
            {
                Id = orderId
            };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Returns(Task.CompletedTask);

            _orderUnitOfWorkMock.Setup(o => o.GetWriteRepository.RemoveAsync(request.Id)).ReturnsAsync(true);
            _orderUnitOfWorkMock.Setup(o => o.SaveAsync()).ReturnsAsync(1);


            // Act
            var result = await _deleteOrderCommandHandler.Handle(request, CancellationToken.None);


            // Assert
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id), Times.Once);
            _orderUnitOfWorkMock.Verify(o => o.GetWriteRepository.RemoveAsync(request.Id), Times.Once);
            _orderUnitOfWorkMock.Verify(o => o.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderCommandHandler_Handle_WhenOrderIdNotExists_ShouldThrowBusinessException()
        {

            // Arrange
            var errorMessage = OrderMessages.OrderIdNotFound;

            var orderId = Guid.NewGuid();
            var request = new DeleteOrderCommandRequest(orderId)
            {
                Id = orderId
            };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _orderBusinessRulesMock.Object.EnsureOrderIdExists(request.Id));

            // Assert
            Assert.Equal(exception.Message, errorMessage);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id), Times.Once);

        }
        [Fact]
        public async Task DeleteOrderCommandHandler_Handle_WhenBusinessRuleFails_ShouldReturnFailResponse()
        {

            // Arrange
            var errorMessage = OrderMessages.OrderIdNotFound;

            var orderId = Guid.NewGuid();
            var request = new DeleteOrderCommandRequest(orderId)
            {
                Id = orderId
            };

            _orderBusinessRulesMock.Setup(o => o.EnsureOrderIdExists(request.Id)).Throws(new BusinessException(errorMessage));

            // Act
            var result = await _deleteOrderCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResponseType.Fail, result.ResponseType);
            Assert.NotNull(result.errorDTO);
            Assert.Contains(result.errorDTO.ValidationErrors, c => c.Message == errorMessage);

            _orderBusinessRulesMock.Verify(o => o.EnsureOrderIdExists(request.Id), Times.Once);

        }
    }
}
