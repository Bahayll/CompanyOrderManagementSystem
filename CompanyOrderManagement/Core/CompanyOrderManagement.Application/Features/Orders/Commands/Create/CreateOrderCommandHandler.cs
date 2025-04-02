using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Orders;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Orders;
using CompanyOrderManagement.Domain.Entities;
using CompanyOrderManagement.Domain.Enums;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, ApiResponse<CreateOrderCommandResponse>>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IOrderBusinessRules _orderBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateOrderCommandHandler(IOrderUnitOfWork orderUnitOfWork, IProductUnitOfWork productUnitOfWork, IOrderBusinessRules orderBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _productUnitOfWork = productUnitOfWork;
            _orderBusinessRules = orderBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreateOrderCommandResponse>> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
 
            try
            {
                await _orderBusinessRules.EnsureCompanyExistsAsync(request.CompanyId);
                await _orderBusinessRules.EnsureUserIsRegisteredForOrderAsync(request.UserId);

                var orders = _mapper.Map<Order>(request);
                List<Product> products = new List<Product>();
               

                foreach (var item in request.ProductsId)
                {
                    var product = await _productUnitOfWork.GetReadRepository.GetByIdAsync(item);

                    if (product.CompanyId == request.CompanyId)
                    {
                        products.Add(product);
                    }
                    else
                    {
                    }
                }
                orders.Products = products;

                orders.OrderStatus = OrderStatus.Processing;
                orders.TotalPrice = (decimal)(orders.UnitPrice * orders.ProductCount)!;

                await _orderUnitOfWork.GetWriteRepository.AddAsync(orders);
                await _orderUnitOfWork.SaveAsync();

                _logger.Info(OrderLogMessage.SuccessfullyCreated(request.Name));
                var response = _mapper.Map<CreateOrderCommandResponse>(orders);
                return ApiResponse<CreateOrderCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(OrderLogMessage.FailedToCreate(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("CreateError", ex.Message) }, true);
                return ApiResponse<CreateOrderCommandResponse>.Fail(error);
            }
            

        }
    }
}
