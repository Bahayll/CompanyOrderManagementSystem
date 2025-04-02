using AutoMapper;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.Queries.GetAll;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using CompanyOrderManagement.Application.Rules.Products;
using CompanyOrderManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CompanyOrderManagement.UnitTest.Products.Queries
{
    public class GetAllProductQueryHandlerTest
    {
        private readonly Mock<IProductUnitOfWork> _productUnitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerServiceMock;
        private readonly GetAllProductQueryHandler _getAllProductQueryHandler;

        public GetAllProductQueryHandlerTest()
        {
            _mapperMock = new Mock<IMapper>();
            _productUnitOfWorkMock = new Mock<IProductUnitOfWork>();
            _loggerServiceMock = new Mock<ILoggerService>();
            _getAllProductQueryHandler = new GetAllProductQueryHandler(_productUnitOfWorkMock.Object, _mapperMock.Object,_loggerServiceMock.Object);
        }

        [Fact]
        public async Task GetAllProductQueryHandler_Handle_ShouldReturnAllProducts()
        {
            // Arrange
            var request = new GetAllProductQueryRequest();
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 1",
                    Description = "Description 1",
                    Stock = 100,
                    Price = 150,
                    CompanyId = Guid.NewGuid(),
                    ProductCategoryId = Guid.NewGuid()
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 2",
                    Description = "Description 2",
                    Stock = 200,
                    Price = 250,
                    CompanyId = Guid.NewGuid(),
                    ProductCategoryId = Guid.NewGuid()
                }
            };

            var response = products.Select(p => new GetAllProductQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedDate = p.CreatedDate,
                LastUpdatedDate = p.LastUpdatedDate.GetValueOrDefault(),
                CompanyId= p.CompanyId, 
                ProductCategoryId= p.ProductCategoryId,
                Price= p.Price,
                Stock= p.Stock,

            }).ToList();

            _productUnitOfWorkMock.Setup(p => p.GetReadRepository.GetAll(It.IsAny<bool>())).Returns(products.AsQueryable());
            _mapperMock.Setup(p => p.Map<List<GetAllProductQueryResponse>>(products)).Returns(response);

            // Act

            var result = await _getAllProductQueryHandler.Handle(request,CancellationToken.None);

            // Assert
            Assert.Equal(ResponseType.Success,result.ResponseType);

            _productUnitOfWorkMock.Verify(p => p.GetReadRepository.GetAll(It.IsAny<bool>()),Times.Once);
            _mapperMock.Verify(p => p.Map<List<GetAllProductQueryResponse>>(products));


        }
    }
}
