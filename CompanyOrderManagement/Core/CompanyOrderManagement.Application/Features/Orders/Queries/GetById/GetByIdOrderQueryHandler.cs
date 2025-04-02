using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Orders;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Orders;
using MediatR;

namespace CompanyOrderManagement.Application.Features.Orders.Queries.GetById
{
    public class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQueryRequest, ApiResponse<GetByIdOrderQueryResponse>>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IOrderBusinessRules _orderBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetByIdOrderQueryHandler(IOrderUnitOfWork orderUnitOfWork, IOrderBusinessRules orderBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _orderBusinessRules = orderBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<GetByIdOrderQueryResponse>> Handle(GetByIdOrderQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderBusinessRules.EnsureOrderIdExists(request.Id);

                var order = await _orderUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                _logger.Info(OrderLogMessage.SuccessfullyFetchedById(request.Id));
                var response = _mapper.Map<GetByIdOrderQueryResponse>(order);
                return ApiResponse<GetByIdOrderQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchByIdError", ex.Message) }, true);
                return ApiResponse<GetByIdOrderQueryResponse>.Fail(error);
            }
        }
    }
}
