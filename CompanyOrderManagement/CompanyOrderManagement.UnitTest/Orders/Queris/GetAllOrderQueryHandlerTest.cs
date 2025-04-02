using AutoMapper;
using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetAll;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Orders.Queris
{
    public class GetAllOrderQueryHandlerTest
    {
        private readonly Mock<IOrderUnitOfWork> _orderUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly GetAllOrderQueryHandler _getAllOrderQuerydHandler;

        public GetAllOrderQueryHandlerTest()
        {
            _orderUnitOfWorkMock = new Mock<IOrderUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _getAllOrderQuerydHandler = new GetAllOrderQueryHandler(_orderUnitOfWorkMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }
        [Fact]
        public async Task GetAllOrderQueryHandler_Handle_ShouldReturnAllOrders()
        {
            //Arrange 
            var request = new GetAllOrderQueryRequest();
            var orders = new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Order",
                    Address = "Test Address",
                    Description = "Description",
                    OrderStatus = OrderStatus.Processing,
                    CompanyId = Guid.NewGuid(),
                    UserId  = Guid.NewGuid(),
                    ProductCount = 100,
                    TotalPrice = 500,
                    UnitPrice = 5,

                },
                 new Order
                {
                    Id = Guid.NewGuid(),
                    Name = "Test1 Order",
                    Address = "Test1 Address",
                    Description = "Description1",
                    OrderStatus = OrderStatus.Processing,
                    CompanyId = Guid.NewGuid(),
                    UserId  = Guid.NewGuid(),
                    ProductCount = 200,
                    TotalPrice = 1000,
                    UnitPrice = 5

                }
            };
            var response = orders.Select(o => new GetAllOrderQueryResponse
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Description = o.Description,
                CreatedDate = o.CreatedDate,
                LastUpdatedDate = o.LastUpdatedDate,
                OrderStatus = o.OrderStatus,
                UnitPrice = o.UnitPrice,
                TotalPrice = o.TotalPrice,
                CompanyId = o.CompanyId,
                ProductCount = o.ProductCount,
                UserId = o.UserId
            }).ToList();

            _orderUnitOfWorkMock.Setup(o => o.GetReadRepository.GetAll(It.IsAny<bool>())).Returns(orders.AsQueryable());
            _mapperMock.Setup(o => o.Map<List<GetAllOrderQueryResponse>>(orders)).Returns(response);

            // Act
            var result = await _getAllOrderQuerydHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(response, result.Data);
            Assert.Equal(ResponseType.Success, result.ResponseType);

            _orderUnitOfWorkMock.Verify(o => o.GetReadRepository.GetAll(It.IsAny<bool>()), Times.Once);
            _mapperMock.Verify(o => o.Map<List<GetAllOrderQueryResponse>>(orders), Times.Once);

        }

    }
}
